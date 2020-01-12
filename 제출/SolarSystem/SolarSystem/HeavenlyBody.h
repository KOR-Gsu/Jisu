#pragma once
#include"define.h"

class HeavenlyBody
{
protected:
	D3DXMATRIXA16	m_matMyMatrix;
	D3DXMATRIXA16	m_matRot;
	D3DXMATRIXA16	m_matRev;
	D3DXMATRIXA16	m_matTrans;
	D3DXVECTOR3		m_vRev;
	D3DXVECTOR3		m_vTrans;
	D3DXVECTOR3		m_vNormal;
	float			m_fRotSpeed;
	float			m_fRevSpeed;
public:
	virtual void	Init(float RotSpeed, float RevSpeed, D3DXVECTOR3 tm) = 0;
	virtual void	Update() = 0;
	D3DXMATRIXA16	GetMatrix() { return  m_matMyMatrix; }
	HeavenlyBody() {};
	virtual ~HeavenlyBody() {};
};

class Sun : public HeavenlyBody
{
public:
	void Init(float RotSpeed, float RevSpeed, D3DXVECTOR3 tm);
	void Update();
	Sun() {};
	~Sun() {};
};

class Earth : public HeavenlyBody
{
public:
	void Init(float RotSpeed, float RevSpeed, D3DXVECTOR3 tm);
	void Update();
	Earth() {};
	~Earth() {};
};

class Moon : public HeavenlyBody
{
public:
	void Init(float RotSpeed, float RevSpeed, D3DXVECTOR3 tm);
	void Update();
	Moon() {};
	~Moon() {};
};