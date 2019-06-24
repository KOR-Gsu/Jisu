#include "PosAndAnchorScene.h"
#include "SimpleAudioEngine.h"

USING_NS_CC;

Scene* PosAndAnchorScene::createScene()
{
	auto scene = Scene::create();
	auto layer = PosAndAnchorScene::create();
	scene->addChild(layer);

	return scene;
}

bool PosAndAnchorScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	Size visibleSize = Director::getInstance()->getVisibleSize();
	Vec2 origin = Director::getInstance()->getVisibleOrigin();

	Sprite* sprite = Sprite::create("HelloWorld.png");
	sprite->setPosition(Vec2(visibleSize.width / 2 + origin.x, visibleSize.height / 2 + origin.y));
	this->addChild(sprite, 0);

	sprite = Sprite::create("grossini.png");
	//sprite->setPosition(ccp(0, 0));
	//sprite->setAnchorPoint(Vec2(0, 0));

	sprite->setPosition(Vec2(visibleSize.width, visibleSize.height));
	sprite->setAnchorPoint(Vec2(1, 1));

	this->addChild(sprite, 0);

	this->schedule(schedule_selector(PosAndAnchorScene::Update));
	this->schedule(schedule_selector(PosAndAnchorScene::UpdatePerSec), 1.0f);

	return true;
}

void PosAndAnchorScene::Update(float eTime)
{
	//Director::getInstance()->replaceScene
	//log("eTime = %f", eTime);
}

void PosAndAnchorScene::UpdatePerSec(float eTime)
{
	log("eTime = %f", eTime);
	//unschedule(schedule_selector(PosAndAnchorScene::UpdatePerSec));
}


