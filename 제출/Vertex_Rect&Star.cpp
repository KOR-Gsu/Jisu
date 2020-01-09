/**========================================================================
 * ���� ������
 * ������ ������ �ϴ� ���� �����ϰ� �ȴ�.
 *=========================================================================*/

 // Direct3D9�� ����ϱ� ���� ���
#include <d3d9.h>/**========================================================================
 * ���� ������
 * ������ ������ �ϴ� ���� �����ϰ� �ȴ�.
 *=========================================================================*/

 // Direct3D9�� ����ϱ� ���� ���
#include <d3d9.h>
#include <d3dx9.h>

/**========================================================================
 * ���� ����
 *=========================================================================*/

LPDIRECT3D9			g_pD3D = NULL;
LPDIRECT3DDEVICE9	g_pd3dDevice = NULL;

LPDIRECT3DVERTEXBUFFER9 g_pVB1 = NULL; //������ ������ ���� ����
LPDIRECT3DVERTEXBUFFER9 g_pVB2 = nullptr;

struct CUSTOMVERTEX
{
	FLOAT x, y, z, rhw; //������ ��ȯ�� ��ǥ(rhw ���� ������ ��ȯ�� �Ϸ�� �����̴�.)
	DWORD color;		//������ ����
};

// ����� ���� ����ü�� ���� ������ ��Ÿ���� FVF��
// ����ü�� x, y, z, RHW ���� Diffuse ���� ������ �̷���� ������ �� �� �ִ�.
// D3DFVF_DIFFUSE �ɼ����� �������� ������ �ִٴ°� �˷��ش�.
#define D3DFVF_CUSTOMVERTEX (D3DFVF_XYZRHW | D3DFVF_DIFFUSE)

/**========================================================================
 * Direct3D �ʱ�ȭ
 *=========================================================================*/
HRESULT InitD3D(HWND hWnd)
{
	// ����̽��� �����ϱ� ���� D3D ��ü ����
	if (NULL == (g_pD3D = Direct3DCreate9(D3D_SDK_VERSION)))
		return E_FAIL;

	D3DPRESENT_PARAMETERS	d3dpp;				// ����̽� ������ ���� ����ü
	ZeroMemory(&d3dpp, sizeof(d3dpp));			// �ݵ�� ZeroMemory() �Լ��� �̸� ����ü�� ������ ������ �Ѵ�.

	d3dpp.Windowed = TRUE;						// â ���� ����
	d3dpp.SwapEffect = D3DSWAPEFFECT_DISCARD;	// ���� ȿ������ SWAP ȿ��
	d3dpp.BackBufferFormat = D3DFMT_UNKNOWN;	// ���� ����ȭ�� ��忡 ���缭 �ĸ� ���۸� ����.

	// ����̽��� �����ؼ� ����
	// ����Ʈ ����ī�带 ����ϰ�, HAL ����̽��� �����Ѵ�.
	// ���� ó���� ��� ī�忡�� �����ϴ� SWó���� �����Ѵ�.
	if (FAILED(g_pD3D->CreateDevice(D3DADAPTER_DEFAULT, D3DDEVTYPE_HAL, hWnd,
		D3DCREATE_SOFTWARE_VERTEXPROCESSING, &d3dpp, &g_pd3dDevice)))
	{
		return E_FAIL;
	}

	// ����̽� ���� ������ ó���� ��� ���⿡�� �Ѵ�.

	return S_OK;
}

/**========================================================================
 * ���� �ʱ�ȭ : ���� ���۸� �����ϰ� �������� ä�� �ִ´�.
 *
 * ���� ���۶� �⺻������ ���� ������ ���� �ִ� �޸� ����̴�.
 * ���� ���۸� ������ �������� �ݵ�� Lock()�� Unlock()���� �����͸� ����
 * ���� ������ ���� ���ۿ� ��־�� �Ѵ�.
 * ���� D3D�� �ε��� ���۵� ��� �����ϴٴ� ���� �������.
 * ���� ���۳� �ε��� ���۴� �⺻ �ý��� �޸� �ܿ� ����̽� �޸�(����ī�� �޸�)��
 * ������ �� �ִµ�, ��κ��� ����ī�忡���� �̷��� �� ��� ��û�� �ӵ�
 * ����� ���� �� �ִ�.
 *=========================================================================*/
HRESULT InitVB()
{
	// �ﰢ���� �������ϱ� ���� 3���� ���� ����
	CUSTOMVERTEX vertices1[] =
	{
		{ 220.0f, 260.0f, 0.5f, 1.0f, 0xffff0000, },
		{ 270.0f, 260.0f, 0.5f, 1.0f, 0xff00ff00, },
		{ 250.0f, 300.0f, 0.5f, 1.0f, 0xff00ffff, },

		{ 220.0f, 340.0f, 0.5f, 1.0f, 0xff00ffff, },
		{ 250.0f, 300.0f, 0.5f, 1.0f, 0xff00ff00, },
		{ 270.0f, 340.0f, 0.5f, 1.0f, 0xff00ffff, },

		{ 270.0f, 340.0f, 0.5f, 1.0f, 0xffff0000, },
		{ 320.0f, 340.0f, 0.5f, 1.0f, 0xffff0000, },
		{ 300.0f, 380.0f, 0.5f, 1.0f, 0xffff0000, },

		{ 330.0f, 340.0f, 0.5f, 1.0f, 0xffff0000, },
		{ 350.0f, 300.0f, 0.5f, 1.0f, 0xffff0000, },
		{ 370.0f, 340.0f, 0.5f, 1.0f, 0xffff0000, },

		{ 350.0f, 300.0f, 0.5f, 1.0f, 0xffff0000, },
		{ 330.0f, 260.0f, 0.5f, 1.0f, 0xff00ffff, },
		{ 370.0f, 260.0f, 0.5f, 1.0f, 0xffff0000, },

		{ 330.0f, 260.0f, 0.5f, 1.0f, 0xff00ffff, },
		{ 270.0f, 260.0f, 0.5f, 1.0f, 0xff00ff00, },
		{ 300.0f, 220.0f, 0.5f, 1.0f, 0xffff0000, },
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

	if (FAILED(g_pd3dDevice->CreateVertexBuffer(18 * sizeof(CUSTOMVERTEX), 0, D3DFVF_CUSTOMVERTEX,
		D3DPOOL_DEFAULT, &g_pVB1, NULL)))
	{
		return E_FAIL;
	}
	void* pVertices1;
	if (FAILED(g_pVB1->Lock(0, sizeof(vertices1), (void**)& pVertices1, 0)))
	{
		return E_FAIL;
	}
	memcpy(pVertices1, vertices1, sizeof(vertices1));
	g_pVB1->Unlock();

	if (FAILED(g_pd3dDevice->CreateVertexBuffer(8 * sizeof(CUSTOMVERTEX), 0, D3DFVF_CUSTOMVERTEX,
		D3DPOOL_DEFAULT, &g_pVB2, NULL)))
	{
		return E_FAIL;
	}
	void* pVertices2;
	if (FAILED(g_pVB2->Lock(0, sizeof(vertices2), (void**)& pVertices2, 0)))
	{
		return E_FAIL;
	}
	memcpy(pVertices2, vertices2, sizeof(vertices2));
	g_pVB2->Unlock();

	return S_OK;
}

/**========================================================================
 * �ʱ�ȭ ��ü�� Release
 *=========================================================================*/
void Cleanup()
{
	//���� ������ �߿� �������̽� ������ �������� ��������.
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
 * ȭ�� �׸���
 *=========================================================================*/
void Render()
{
	if (NULL == g_pd3dDevice)
		return;

	// �ĸ� ���۸� �Ķ���(0, 0, 255)���� �����.
	g_pd3dDevice->Clear(0, NULL, D3DCLEAR_TARGET, D3DCOLOR_XRGB(0, 0, 255), 1.0f, 0);

	// ������ ����
	if (SUCCEEDED(g_pd3dDevice->BeginScene()))
	{
		// ���� ������ �ﰢ���� �׸���.

		// ���� ������ ��� �ִ� ���� ���۸� ��� ��Ʈ������ �Ҵ��Ѵ�.
		g_pd3dDevice->SetStreamSource(0, g_pVB1, 0, sizeof(CUSTOMVERTEX));
		// D3D���� ���� ���̴� ������ �����Ѵ�. ��κ��� ��쿡�� FVF�� �����Ѵ�.
		g_pd3dDevice->SetFVF(D3DFVF_CUSTOMVERTEX);

		// ���� ������ ����ϱ� ���� DrawPrimitive() �Լ��� ȣ���Ѵ�.
		g_pd3dDevice->DrawPrimitive(D3DPT_TRIANGLELIST, 0, 12);

		g_pd3dDevice->SetStreamSource(0, g_pVB2, 0, sizeof(CUSTOMVERTEX));
		// D3D���� ���� ���̴� ������ �����Ѵ�. ��κ��� ��쿡�� FVF�� �����Ѵ�.
		g_pd3dDevice->SetFVF(D3DFVF_CUSTOMVERTEX);

		// ���� ������ ����ϱ� ���� DrawPrimitive() �Լ��� ȣ���Ѵ�.
		g_pd3dDevice->DrawPrimitive(D3DPT_TRIANGLEFAN, 0, 6);

		//������ ����
		g_pd3dDevice->EndScene();
	}

	// �ĸ� ���۸� ���̴� ȭ������ ��ȯ.
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
 * Window ����
 *=========================================================================*/
INT WINAPI WinMain(HINSTANCE hInst, HINSTANCE, LPSTR, INT)
{
	//������ Ŭ���� ���
	WNDCLASSEX wc = { sizeof(WNDCLASSEX), CS_CLASSDC, MsgProc, 0L, 0L,
		GetModuleHandle(NULL), NULL, NULL, NULL, NULL,
		"D3D Vertices", NULL };

	//winclass �������Ϳ� ���
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

	//��ϵ� ������Ʈ winclass ������.
	UnregisterClass("D3D Vertices", wc.hInstance);
	return 0;
}
#include <d3dx9.h>

/**========================================================================
 * ���� ����
 *=========================================================================*/

LPDIRECT3D9			g_pD3D = NULL;
LPDIRECT3DDEVICE9	g_pd3dDevice = NULL;

LPDIRECT3DVERTEXBUFFER9 g_pVB1 = NULL; //������ ������ ���� ����
LPDIRECT3DVERTEXBUFFER9 g_pVB2 = nullptr;

struct CUSTOMVERTEX
{
	FLOAT x, y, z, rhw; //������ ��ȯ�� ��ǥ(rhw ���� ������ ��ȯ�� �Ϸ�� �����̴�.)
	DWORD color;		//������ ����
};

// ����� ���� ����ü�� ���� ������ ��Ÿ���� FVF��
// ����ü�� x, y, z, RHW ���� Diffuse ���� ������ �̷���� ������ �� �� �ִ�.
// D3DFVF_DIFFUSE �ɼ����� �������� ������ �ִٴ°� �˷��ش�.
#define D3DFVF_CUSTOMVERTEX (D3DFVF_XYZRHW | D3DFVF_DIFFUSE)

/**========================================================================
 * Direct3D �ʱ�ȭ
 *=========================================================================*/
HRESULT InitD3D(HWND hWnd)
{
	// ����̽��� �����ϱ� ���� D3D ��ü ����
	if (NULL == (g_pD3D = Direct3DCreate9(D3D_SDK_VERSION)))
		return E_FAIL;

	D3DPRESENT_PARAMETERS	d3dpp;				// ����̽� ������ ���� ����ü
	ZeroMemory(&d3dpp, sizeof(d3dpp));			// �ݵ�� ZeroMemory() �Լ��� �̸� ����ü�� ������ ������ �Ѵ�.

	d3dpp.Windowed = TRUE;						// â ���� ����
	d3dpp.SwapEffect = D3DSWAPEFFECT_DISCARD;	// ���� ȿ������ SWAP ȿ��
	d3dpp.BackBufferFormat = D3DFMT_UNKNOWN;	// ���� ����ȭ�� ��忡 ���缭 �ĸ� ���۸� ����.

	// ����̽��� �����ؼ� ����
	// ����Ʈ ����ī�带 ����ϰ�, HAL ����̽��� �����Ѵ�.
	// ���� ó���� ��� ī�忡�� �����ϴ� SWó���� �����Ѵ�.
	if (FAILED(g_pD3D->CreateDevice(D3DADAPTER_DEFAULT, D3DDEVTYPE_HAL, hWnd,
		D3DCREATE_SOFTWARE_VERTEXPROCESSING, &d3dpp, &g_pd3dDevice)))
	{
		return E_FAIL;
	}

	// ����̽� ���� ������ ó���� ��� ���⿡�� �Ѵ�.

	return S_OK;
}

/**========================================================================
 * ���� �ʱ�ȭ : ���� ���۸� �����ϰ� �������� ä�� �ִ´�.
 *
 * ���� ���۶� �⺻������ ���� ������ ���� �ִ� �޸� ����̴�.
 * ���� ���۸� ������ �������� �ݵ�� Lock()�� Unlock()���� �����͸� ����
 * ���� ������ ���� ���ۿ� ��־�� �Ѵ�.
 * ���� D3D�� �ε��� ���۵� ��� �����ϴٴ� ���� �������.
 * ���� ���۳� �ε��� ���۴� �⺻ �ý��� �޸� �ܿ� ����̽� �޸�(����ī�� �޸�)��
 * ������ �� �ִµ�, ��κ��� ����ī�忡���� �̷��� �� ��� ��û�� �ӵ�
 * ����� ���� �� �ִ�.
 *=========================================================================*/
HRESULT InitVB()
{
	// �ﰢ���� �������ϱ� ���� 3���� ���� ����
	CUSTOMVERTEX vertices1[] =
	{
		{ 220.0f, 260.0f, 0.5f, 1.0f, 0xffff0000, },
		{ 270.0f, 260.0f, 0.5f, 1.0f, 0xff00ff00, },
		{ 250.0f, 300.0f, 0.5f, 1.0f, 0xff00ffff, },

		{ 250.0f, 300.0f, 0.5f, 1.0f, 0xff00ffff, },
		{ 270.0f, 340.0f, 0.5f, 1.0f, 0xff00ffff, },
		{ 220.0f, 350.0f, 0.5f, 1.0f, 0xff00ffff, },

		{ 270.0f, 340.0f, 0.5f, 1.0f, 0xffff0000, },
		{ 320.0f, 340.0f, 0.5f, 1.0f, 0xffff0000, },
		{ 300.0f, 380.0f, 0.5f, 1.0f, 0xffff0000, },

		{ 330.0f, 340.0f, 0.5f, 1.0f, 0xffff0000, },
		{ 350.0f, 300.0f, 0.5f, 1.0f, 0xffff0000, },
		{ 370.0f, 340.0f, 0.5f, 1.0f, 0xffff0000, },

		{ 350.0f, 300.0f, 0.5f, 1.0f, 0xffff0000, },
		{ 330.0f, 260.0f, 0.5f, 1.0f, 0xff00ffff, },
		{ 370.0f, 260.0f, 0.5f, 1.0f, 0xffff0000, },

		{ 330.0f, 260.0f, 0.5f, 1.0f, 0xff00ffff, },
		{ 270.0f, 260.0f, 0.5f, 1.0f, 0xff00ff00, },
		{ 300.0f, 220.0f, 0.5f, 1.0f, 0xffff0000, },
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
	if (FAILED(g_pVB1->Lock(0, sizeof(vertices1), (void**)& pVertices1, 0)))
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
	if (FAILED(g_pVB2->Lock(0, sizeof(vertices2), (void**)& pVertices2, 0)))
	{
		return E_FAIL;
	}
	memcpy(pVertices2, vertices2, sizeof(vertices2));
	g_pVB2->Unlock();

	return S_OK;
}

/**========================================================================
 * �ʱ�ȭ ��ü�� Release
 *=========================================================================*/
void Cleanup()
{
	//���� ������ �߿� �������̽� ������ �������� ��������.
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
 * ȭ�� �׸���
 *=========================================================================*/
void Render()
{
	if (NULL == g_pd3dDevice)
		return;

	// �ĸ� ���۸� �Ķ���(0, 0, 255)���� �����.
	g_pd3dDevice->Clear(0, NULL, D3DCLEAR_TARGET, D3DCOLOR_XRGB(0, 0, 255), 1.0f, 0);

	// ������ ����
	if (SUCCEEDED(g_pd3dDevice->BeginScene()))
	{
		// ���� ������ �ﰢ���� �׸���.

		// ���� ������ ��� �ִ� ���� ���۸� ��� ��Ʈ������ �Ҵ��Ѵ�.
		g_pd3dDevice->SetStreamSource(0, g_pVB1, 0, sizeof(CUSTOMVERTEX));
		// D3D���� ���� ���̴� ������ �����Ѵ�. ��κ��� ��쿡�� FVF�� �����Ѵ�.
		g_pd3dDevice->SetFVF(D3DFVF_CUSTOMVERTEX);

		// ���� ������ ����ϱ� ���� DrawPrimitive() �Լ��� ȣ���Ѵ�.
		g_pd3dDevice->DrawPrimitive(D3DPT_TRIANGLESTRIP, 0, 6);

		g_pd3dDevice->SetStreamSource(0, g_pVB2, 0, sizeof(CUSTOMVERTEX));
		// D3D���� ���� ���̴� ������ �����Ѵ�. ��κ��� ��쿡�� FVF�� �����Ѵ�.
		g_pd3dDevice->SetFVF(D3DFVF_CUSTOMVERTEX);

		// ���� ������ ����ϱ� ���� DrawPrimitive() �Լ��� ȣ���Ѵ�.
		g_pd3dDevice->DrawPrimitive(D3DPT_TRIANGLEFAN, 0, 6);

		//������ ����
		g_pd3dDevice->EndScene();
	}

	// �ĸ� ���۸� ���̴� ȭ������ ��ȯ.
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
 * Window ����
 *=========================================================================*/
INT WINAPI WinMain(HINSTANCE hInst, HINSTANCE, LPSTR, INT)
{
	//������ Ŭ���� ���
	WNDCLASSEX wc = { sizeof(WNDCLASSEX), CS_CLASSDC, MsgProc, 0L, 0L,
		GetModuleHandle(NULL), NULL, NULL, NULL, NULL,
		"D3D Vertices", NULL };

	//winclass �������Ϳ� ���
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

	//��ϵ� ������Ʈ winclass ������.
	UnregisterClass("D3D Vertices", wc.hInstance);
	return 0;
}