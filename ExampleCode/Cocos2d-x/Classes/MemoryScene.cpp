#include "MemoryScene.h"
#include "SimpleAudioEngine.h"

USING_NS_CC;

Scene* MemoryScene::createScene()
{
	auto scene = Scene::create();
	auto layer = MemoryScene::create();
	scene->addChild(layer);

	return scene;
}

bool MemoryScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	//m_pSprite = new Sprite();
	//m_pSprite->initWithFile("grossiniaa.png");
	m_pSprite = Sprite::create("grossiniaa.png");

	m_pSprite->setPosition(Vec2(300, 300));
	log("Ref Count = %d", m_pSprite->getReferenceCount());
	addChild(m_pSprite);
	log("Ref Count = %d", m_pSprite->getReferenceCount());
	//m_pSprite->release();
	//log("Ref Count = %d", m_pSprite->getReferenceCount());

	//Ref �� 2�̸� Layer Release�� -- AutoReleasePool Relase -- �ؼ� ���� ���簡 ��
	//����� create���� new�� �޴ٸ� autorelase�� ȣ���Ͽ� AutoReleasePool�� �������
	


	LabelTTF* pLabel = LabelTTF::create("asdasd", "fonts\\arial.ttf", 30);
	pLabel->setPosition(Vec2(100, 100));
	addChild(pLabel);

	return true;
}

