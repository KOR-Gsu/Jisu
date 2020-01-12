#pragma once
#include"HeavenlyBody.h"

class SolarSystem
{
private:
	CUSTOMVERTEX	m_vtxCube[8] =
	{
		{-1, 1, 1, 0xffff0000},
		{ 1, 1, 1, 0xff00ff00},
		{ 1, 1,-1, 0xff0000ff},
		{-1, 1,-1, 0xffffff00},

		{-1,-1, 1, 0xffff0000},
		{ 1,-1, 1, 0xffff0000},
		{ 1,-1,-1, 0xffff0000},
		{-1,-1,-1, 0xffff0000},
	};

	MYINDEX m_idxCube[12] =
	{
		{0, 1, 2}, {0, 2, 3},
		{4, 6, 5}, {4, 7, 6},
		{0, 3, 7}, {0, 7, 4},
		{1, 5, 6}, {1, 6, 2},
		{3, 2, 6}, {3, 6, 7},
		{0, 4, 5}, {0, 5, 1},
	};

	LPDIRECT3D9				m_pD3D = NULL;
	LPDIRECT3DDEVICE9		m_pD3DDevice = NULL;

	LPDIRECT3DVERTEXBUFFER9	m_pVB = NULL;
	LPDIRECT3DINDEXBUFFER9	m_pIB = NULL;

	std::list<HeavenlyBody*> m_ltHeavenlyBodyList;

	bool InitVB();
	bool InitIB();
	void SetupCamera();
	void SetupHeavenlyBody();
public:
	bool Init(HWND hWnd);
	void Update();
	void DrawMesh(D3DXMATRIX* pMat);
	void Render();
	void CleanUp();
	SolarSystem();
	~SolarSystem();
};

