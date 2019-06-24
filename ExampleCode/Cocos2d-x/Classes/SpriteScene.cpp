#include "SpriteScene.h"
#include "SimpleAudioEngine.h"

USING_NS_CC;

Scene* SpriteScene::createScene()
{
	auto scene = Scene::create();
	auto layer = SpriteScene::create();
	scene->addChild(layer);

	return scene;
}

bool SpriteScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	
	Sprite* pSprite = Sprite::create("grossini.png");
	pSprite->setPosition(Vec2(300, 500));
	//pSprite->setRotation(45.0f);
	addChild(pSprite, 3);

	//pSprite = Sprite::create("grossinis_sister1.png");
	//pSprite->setPosition(Vec2(120, 120));
	//addChild(pSprite, 2);

	Sprite* pHPSprite = Sprite::create("white-512x512.png");
	pHPSprite->setAnchorPoint(Vec2(0.5f, 0.5f));
	pHPSprite->setPosition(pSprite->getContentSize().width / 2, pSprite->getContentSize().height + 15);
	pHPSprite->setColor(Color3B(0, 255, 0));
	pHPSprite->setTextureRect(Rect(0, 0, 50, 10));
	pSprite->addChild(pHPSprite, 2);
	

	for (int i = 0; i < 12; i++)
	{
		Sprite* pSprite = Sprite::create("img.png");
		pSprite->setPosition(Vec2(100 * i, 0));
		pSprite->setAnchorPoint(Vec2(0, 0));
		//pSprite->setRotation(45.0f);
		addChild(pSprite);
	}

	return true;
}