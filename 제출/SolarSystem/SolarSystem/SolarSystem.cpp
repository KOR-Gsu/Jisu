#include "SolarSystem.h"


bool SolarSystem::InitVB()
{
	if (FAILED(m_pD3DDevice->CreateVertexBuffer(8 * sizeof(CUSTOMVERTEX), 0, D3DFVF_CUSTOMVERTEX,
		D3DPOOL_DEFAULT, &m_pVB, NULL)))
	{
		return E_FAIL;
	}

	void* pVertices;

	if (FAILED(m_pVB->Lock(0, sizeof(m_vtxCube), (void**)&pVertices, 0)))
		return E_FAIL;

	memcpy(pVertices, m_vtxCube, sizeof(m_vtxCube));

	m_pVB->Unlock();

	return S_OK;
}

bool SolarSystem::InitIB()
{
	if (FAILED(m_pD3DDevice->CreateIndexBuffer(12 * sizeof(MYINDEX), 0, D3DFMT_INDEX16, D3DPOOL_DEFAULT, &m_pIB, NULL)))
	{
		return E_FAIL;
	}

	void** pIndices;

	if (FAILED(m_pIB->Lock(0, sizeof(m_idxCube), (void**)&pIndices, 0)))
	{
		return E_FAIL;
	}

	memcpy(pIndices, m_idxCube, sizeof(m_idxCube));

	m_pIB->Unlock();

	return S_OK;
}

bool SolarSystem::Init(HWND hWnd)
{
	if (NULL == (m_pD3D = Direct3DCreate9(D3D_SDK_VERSION)))
	{
		return E_FAIL;
	}

	D3DPRESENT_PARAMETERS d3dpp;
	ZeroMemory(&d3dpp, sizeof(d3dpp));

	d3dpp.Windowed = TRUE;
	d3dpp.SwapEffect = D3DSWAPEFFECT_DISCARD;
	d3dpp.BackBufferFormat = D3DFMT_UNKNOWN;
	d3dpp.EnableAutoDepthStencil = TRUE;
	d3dpp.AutoDepthStencilFormat = D3DFMT_D16;

	if (FAILED(m_pD3D->CreateDevice(D3DADAPTER_DEFAULT, D3DDEVTYPE_HAL, hWnd,
		D3DCREATE_SOFTWARE_VERTEXPROCESSING,
		&d3dpp, &m_pD3DDevice)))
	{
		return E_FAIL;
	}

	m_pD3DDevice->SetRenderState(D3DRS_CULLMODE, D3DCULL_CCW);

	m_pD3DDevice->SetRenderState(D3DRS_ZENABLE, TRUE);

	m_pD3DDevice->SetRenderState(D3DRS_LIGHTING, FALSE);

	if (FAILED(InitVB()))
		return E_FAIL;

	if (FAILED(InitIB()))
		return E_FAIL;

	SetupCamera();

	SetupHeavenlyBody();

	return S_OK;
}

void SolarSystem::Update()
{
	for (auto iter = m_ltHeavenlyBodyList.begin(); iter != m_ltHeavenlyBodyList.end(); iter++)
		(*iter)->Update();
}

void SolarSystem::DrawMesh(D3DXMATRIX* pMat)
{
	m_pD3DDevice->SetTransform(D3DTS_WORLD, pMat);
	m_pD3DDevice->SetStreamSource(0, m_pVB, 0, sizeof(CUSTOMVERTEX));
	m_pD3DDevice->SetFVF(D3DFVF_CUSTOMVERTEX);
	m_pD3DDevice->SetIndices(m_pIB);
	m_pD3DDevice->DrawIndexedPrimitive(D3DPT_TRIANGLELIST, 0, 0, 8, 0, 12);
}

void SolarSystem::SetupCamera()
{
	D3DXMATRIXA16 matWorld;
	D3DXMatrixIdentity(&matWorld);
	m_pD3DDevice->SetTransform(D3DTS_WORLD, &matWorld);

	D3DXVECTOR3 vEyePt(0.0f, 10.0f, -10.0f);
	D3DXVECTOR3 vLookatPt(0.0f, 0.0f, 0.0f);
	D3DXVECTOR3 vUpVec(0.0f, 1.0f, 0.0f);

	D3DXMATRIXA16 matView;
	D3DXMatrixLookAtLH(&matView, &vEyePt, &vLookatPt, &vUpVec);
	m_pD3DDevice->SetTransform(D3DTS_VIEW, &matView);

	D3DXMATRIXA16 matProj;
	D3DXMatrixPerspectiveFovLH(&matProj, D3DX_PI / 2, 1.0f, 1.0f, 100.0f);
	m_pD3DDevice->SetTransform(D3DTS_PROJECTION, &matProj);
}

void SolarSystem::SetupHeavenlyBody()
{
	Sun* sun = new Sun;
	sun->Init(500.0f, 0.0f, D3DXVECTOR3(0, 0, 0));
	m_ltHeavenlyBodyList.push_back(sun);

	Earth* earth = new Earth;
	earth->Init(500.0f, 800.0f, D3DXVECTOR3(9.0f, 0, 0));
	m_ltHeavenlyBodyList.push_back(earth);

	Moon* moon = new Moon;
	moon->Init(500.0f, 50.0f, D3DXVECTOR3(2.5f, 0, 0));
	m_ltHeavenlyBodyList.push_back(moon);
}

void SolarSystem::Render()
{
	D3DXMATRIXA16 matWorld;
	D3DXMatrixIdentity(&matWorld);

	m_pD3DDevice->Clear(0, NULL, D3DCLEAR_TARGET | D3DCLEAR_ZBUFFER, D3DCOLOR_XRGB(0, 0, 0), 1.0f, 0);

	if (SUCCEEDED(m_pD3DDevice->BeginScene()))
	{
		for (auto iter = m_ltHeavenlyBodyList.begin(); iter != m_ltHeavenlyBodyList.end(); iter++)
		{
			matWorld = (*iter)->GetMatrix() * matWorld;
			DrawMesh(&matWorld);
		}

		m_pD3DDevice->EndScene();
	}

	m_pD3DDevice->Present(NULL, NULL, NULL, NULL);
}

void SolarSystem::CleanUp()
{
	if (m_pIB != NULL)
		m_pIB->Release();

	if (m_pVB != NULL)
		m_pVB->Release();

	if (m_pD3DDevice != NULL)
		m_pD3DDevice->Release();

	if (m_pD3D != NULL)
		m_pD3D->Release();
}

SolarSystem::SolarSystem()
{
	m_pD3D = NULL;
	m_pD3DDevice = NULL;
	m_pVB = NULL;
	m_pIB = NULL;
}


SolarSystem::~SolarSystem()
{
	for (auto iter = m_ltHeavenlyBodyList.begin(); iter != m_ltHeavenlyBodyList.end(); iter++)
	{
		auto del = iter;
		iter = m_ltHeavenlyBodyList.erase(iter);
		delete *del;
	}
}
