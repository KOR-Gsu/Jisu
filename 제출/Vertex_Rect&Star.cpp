/**========================================================================
 * 정점 랜더링
 * 정점을 랜더링 하는 것을 공부하게 된다.
 *=========================================================================*/

 // Direct3D9을 사용하기 위한 헤더
#include <d3d9.h>
#include <d3dx9.h>

/**========================================================================
 * 전역 변수
 *=========================================================================*/

LPDIRECT3D9			g_pD3D = NULL;
LPDIRECT3DDEVICE9	g_pd3dDevice = NULL;

LPDIRECT3DVERTEXBUFFER9 g_pVB1 = NULL; //정점을 보관할 정점 버퍼
LPDIRECT3DVERTEXBUFFER9 g_pVB2 = nullptr;

struct CUSTOMVERTEX
{
	FLOAT x, y, z, rhw; //정점의 변환된 좌표(rhw 값이 있으면 변환이 완료된 정점이다.)
	DWORD color;		//정점의 색깔
};

// 사용자 정점 구조체에 관한 정보를 나타내는 FVF값
// 구조체는 x, y, z, RHW 값과 Diffuse 색깔 값으로 이루어져 있음을 알 수 있다.
// D3DFVF_DIFFUSE 옵션으로 꼭지점이 정해져 있다는걸 알려준다.
#define D3DFVF_CUSTOMVERTEX (D3DFVF_XYZRHW | D3DFVF_DIFFUSE)

/**========================================================================
 * Direct3D 초기화
 *=========================================================================*/
HRESULT InitD3D(HWND hWnd)
{
	// 디바이스를 생성하기 위한 D3D 객체 생성
	if (NULL == (g_pD3D = Direct3DCreate9(D3D_SDK_VERSION)))
		return E_FAIL;

	D3DPRESENT_PARAMETERS	d3dpp;				// 디바이스 생성을 위한 구조체
	ZeroMemory(&d3dpp, sizeof(d3dpp));			// 반드시 ZeroMemory() 함수로 미리 구조체를 깨끗이 지워야 한다.

	d3dpp.Windowed = TRUE;						// 창 모드로 생성
	d3dpp.SwapEffect = D3DSWAPEFFECT_DISCARD;	// 가장 효율적인 SWAP 효과
	d3dpp.BackBufferFormat = D3DFMT_UNKNOWN;	// 현재 바탕화면 모드에 맞춰서 후면 버퍼를 생성.

	// 디바이스를 설정해서 생성
	// 디폴트 비디오카드를 사용하고, HAL 디바이스를 생성한다.
	// 정점 처리는 모든 카드에서 지원하는 SW처리로 생성한다.
	if (FAILED(g_pD3D->CreateDevice(D3DADAPTER_DEFAULT, D3DDEVTYPE_HAL, hWnd,
		D3DCREATE_SOFTWARE_VERTEXPROCESSING, &d3dpp, &g_pd3dDevice)))
	{
		return E_FAIL;
	}

	// 디바이스 상태 정보를 처리할 경우 여기에서 한다.

	return S_OK;
}

/**========================================================================
 * 정점 초기화 : 정점 버퍼를 생성하고 정점값을 채워 넣는다.
 *
 * 정점 버퍼란 기본적으로 정점 정보를 갖고 있는 메모리 블록이다.
 * 정점 버퍼를 생성한 다음에는 반드시 Lock()과 Unlock()으로 포인터를 얻어내서
 * 정점 정보를 정점 버퍼에 써넣어야 한다.
 * 또한 D3D는 인덱스 버퍼도 사용 가능하다는 것을 명심하자.
 * 정점 버퍼나 인덱스 버퍼는 기본 시스템 메모리 외에 디바이스 메모리(비디오카드 메모리)에
 * 생성될 수 있는데, 대부분의 비디오카드에서는 이렇게 할 경우 엄청난 속도
 * 향상을 얻을 수 있다.
 *=========================================================================*/
HRESULT InitVB()
{
	// 삼각형을 랜더링하기 위해 3개의 정점 선언
	CUSTOMVERTEX vertices1[] =
	{
		{ 220.0f, 260.0f, 0.5f, 1.0f, 0xffff0000, },
		{ 270.0f, 260.0f, 0.5f, 1.0f, 0xff00ff00, },
		{ 250.0f, 300.0f, 0.5f, 1.0f, 0xff00ffff, },

		{ 250.0f, 300.0f, 0.5f, 1.0f, 0xff00ffff, },
		{ 270.0f, 340.0f, 0.5f, 1.0f, 0xff00ffff, },
		{ 220.0f, 350.0f, 0.5f, 1.0f, 0xff00ffff, },

		{ 310.0f, 200.0f, 0.5f, 1.0f, 0xffff0000, },

		{ 160.0f, 90.0f, 0.5f, 1.0f, 0xffff0000, },
	};
	CUSTOMVERTEX vertices2[] =
	{
		{ 300.0f, 300.0f, 0.5f, 1.0f, 0xffff0000, },

		{ 270.0f, 260.0f, 0.5f, 1.0f, 0xff00ff00, },

		{ 330.0f, 260.0f, 0.5f, 1.0f, 0xff00ffff, },

		{ 350.0f, 300.0f, 0.5f, 1.0f, 0xffff0000, },

		{ 330.0f, 340.0f, 0.5f, 1.0f, 0xffff0000, },

		{ 270.0f, 340.0f, 0.5f, 1.0f, 0xffff0000, },

		{ 250.0f, 300.0f, 0.5f, 1.0f, 0xffff0000, },

		{ 270.0f, 260.0f, 0.5f, 1.0f, 0xff00ff00, },
	};

	if (FAILED(g_pd3dDevice->CreateVertexBuffer(6 * sizeof(CUSTOMVERTEX), 0, D3DFVF_CUSTOMVERTEX,
		D3DPOOL_DEFAULT, &g_pVB1, NULL)))
	{
		return E_FAIL;
	}
	void* pVertices1;
	if (FAILED(g_pVB1->Lock(0, sizeof(vertices1), (void**)&pVertices1, 0)))
	{
		return E_FAIL;
	}
	memcpy(pVertices1, vertices1, sizeof(vertices1));
	g_pVB1->Unlock();

	if (FAILED(g_pd3dDevice->CreateVertexBuffer(7 * sizeof(CUSTOMVERTEX), 0, D3DFVF_CUSTOMVERTEX,
		D3DPOOL_DEFAULT, &g_pVB2, NULL)))
	{
		return E_FAIL;
	}
	void* pVertices2;
	if (FAILED(g_pVB2->Lock(0, sizeof(vertices2), (void**)&pVertices2, 0)))
	{
		return E_FAIL;
	}
	memcpy(pVertices2, vertices2, sizeof(vertices2));
	g_pVB2->Unlock();

	return S_OK;
}

/**========================================================================
 * 초기화 객체들 Release
 *=========================================================================*/
void Cleanup()
{
	//해제 순서가 중요 인터페이스 생성의 역순으로 해제하자.
	if (g_pVB2 != NULL)
		g_pVB2->Release();

	if (g_pVB1 != NULL)
		g_pVB1->Release();

	if (g_pd3dDevice != NULL)
		g_pd3dDevice->Release();

	if (g_pD3D != NULL)
		g_pD3D->Release();
}

/**========================================================================
 * 화면 그리기
 *=========================================================================*/
void Render()
{
	if (NULL == g_pd3dDevice)
		return;

	// 후면 버퍼를 파란색(0, 0, 255)으로 지운다.
	g_pd3dDevice->Clear(0, NULL, D3DCLEAR_TARGET, D3DCOLOR_XRGB(0, 0, 255), 1.0f, 0);

	// 랜더링 시작
	if (SUCCEEDED(g_pd3dDevice->BeginScene()))
	{
		// 정점 버퍼의 삼각형을 그린다.

		// 정점 정보가 담겨 있는 정점 버퍼를 출력 스트림으로 할당한다.
		g_pd3dDevice->SetStreamSource(0, g_pVB1, 0, sizeof(CUSTOMVERTEX));
		// D3D에게 정점 셰이더 정보를 지정한다. 대부분의 경우에는 FVF만 지정한다.
		g_pd3dDevice->SetFVF(D3DFVF_CUSTOMVERTEX);

		// 기하 정보를 출력하기 위한 DrawPrimitive() 함수를 호출한다.
		g_pd3dDevice->DrawPrimitive(D3DPT_TRIANGLESTRIP, 0, 6);

		g_pd3dDevice->SetStreamSource(0, g_pVB2, 0, sizeof(CUSTOMVERTEX));
		// D3D에게 정점 셰이더 정보를 지정한다. 대부분의 경우에는 FVF만 지정한다.
		g_pd3dDevice->SetFVF(D3DFVF_CUSTOMVERTEX);

		// 기하 정보를 출력하기 위한 DrawPrimitive() 함수를 호출한다.
		g_pd3dDevice->DrawPrimitive(D3DPT_TRIANGLEFAN, 0, 6);

		//렌더링 종료
		g_pd3dDevice->EndScene();
	}

	// 후면 버퍼를 보이는 화면으로 전환.
	g_pd3dDevice->Present(NULL, NULL, NULL, NULL);
}

/**========================================================================
 * WinProc
 *=========================================================================*/
LRESULT WINAPI MsgProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg)
	{
	case WM_DESTROY:
		Cleanup();
		PostQuitMessage(0);
		return 0;
	}

	return DefWindowProc(hWnd, msg, wParam, lParam);
}

/**========================================================================
 * Window 생성
 *=========================================================================*/
INT WINAPI WinMain(HINSTANCE hInst, HINSTANCE, LPSTR, INT)
{
	//윈도우 클래스 등록
	WNDCLASSEX wc = { sizeof(WNDCLASSEX), CS_CLASSDC, MsgProc, 0L, 0L,
		GetModuleHandle(NULL), NULL, NULL, NULL, NULL,
		"D3D Vertices", NULL };

	//winclass 레지스터에 등록
	RegisterClassEx(&wc);

	HWND hWnd = CreateWindow("D3D Vertices", "D3D Vertices", WS_OVERLAPPEDWINDOW, 100, 100, 1024, 768,
		GetDesktopWindow(), NULL, NULL, wc.hInstance, NULL);

	if (SUCCEEDED(InitD3D(hWnd)))
	{
		if (SUCCEEDED(InitVB()))
		{
			ShowWindow(hWnd, SW_SHOWDEFAULT);
			UpdateWindow(hWnd);

			MSG msg;
			ZeroMemory(&msg, sizeof(msg));

			while (msg.message != WM_QUIT)
			{
				if (PeekMessage(&msg, NULL, 0U, 0U, PM_REMOVE))
				{
					TranslateMessage(&msg);
					DispatchMessage(&msg);
				}
				else
					Render();
			}
		}
	}

	//등록된 레지스트 winclass 릴리즈.
	UnregisterClass("D3D Vertices", wc.hInstance);
	return 0;
}