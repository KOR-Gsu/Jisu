#include "HeavenlyBody.h"



void Sun::Init(float RotSpeed, float RevSpeed, D3DXVECTOR3 tm)
{
	m_fRotSpeed = RotSpeed;
	m_fRevSpeed = RevSpeed;
	m_vRev = D3DXVECTOR3(0, 1, 0);
	m_vTrans = tm;
	m_vNormal = D3DXVECTOR3(0, 0, -1);

	D3DXMatrixIdentity(&m_matRev);
}

void Sun::Update()
{
	D3DXMatrixRotationY(&m_matRot, GetTickCount() / m_fRotSpeed);
	D3DXMatrixTranslation(&m_matTrans, m_vTrans.x, m_vTrans.y, m_vTrans.z);

	m_matMyMatrix = m_matRot * m_matTrans * m_matRev;
}

void Earth::Init(float RotSpeed, float RevSpeed, D3DXVECTOR3 tm)
{
	m_fRotSpeed = RotSpeed;
	m_fRevSpeed = RevSpeed;
	m_vTrans = tm;
	m_vNormal = D3DXVECTOR3(0, 1, 0);
}

void Earth::Update()
{
	D3DXVec3Normalize(&m_vRev, &D3DXVECTOR3(m_matMyMatrix._11, m_matMyMatrix._22, m_matMyMatrix._33));

	D3DXMatrixRotationY(&m_matRot, GetTickCount() / m_fRotSpeed);
	D3DXMatrixTranslation(&m_matTrans, m_vTrans.x, m_vTrans.y, m_vTrans.z);
	D3DXMatrixRotationY(&m_matRev, GetTickCount() / m_fRevSpeed);

	m_matMyMatrix = m_matRot * m_matTrans * m_matRev;
}

void Moon::Init(float RotSpeed, float RevSpeed, D3DXVECTOR3 tm)
{
	m_fRotSpeed = RotSpeed;
	m_fRevSpeed = RevSpeed;
	m_vRev = D3DXVECTOR3(0, 1, 0);
	m_vTrans = tm;
	m_vNormal = D3DXVECTOR3(0, 0, -1);
}

void Moon::Update()
{
	D3DXVec3Normalize(&m_vRev, &D3DXVECTOR3(m_matMyMatrix._11, m_matMyMatrix._22, m_matMyMatrix._33));

	D3DXMatrixRotationY(&m_matRot, GetTickCount() / m_fRotSpeed);
	D3DXMatrixTranslation(&m_matTrans, m_vTrans.x, m_vTrans.y, m_vTrans.z);
	D3DXMatrixRotationY(&m_matRev, GetTickCount() / m_fRevSpeed);

	m_matMyMatrix = m_matRot * m_matTrans * m_matRev;
}
