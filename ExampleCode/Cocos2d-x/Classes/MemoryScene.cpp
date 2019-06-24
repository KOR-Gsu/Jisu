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

	//Ref 가 2이면 Layer Release시 -- AutoReleasePool Relase -- 해서 정상 해재가 됨
	//결론은 create쓰고 new를 햇다면 autorelase를 호출하여 AutoReleasePool에 등록하자
	


	LabelTTF* pLabel = LabelTTF::create("asdasd", "fonts\\arial.ttf", 30);
	pLabel->setPosition(Vec2(100, 100));
	addChild(pLabel);

	return true;
}

