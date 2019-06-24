#include "AniamtionScene.h"
#include "SimpleAudioEngine.h"

USING_NS_CC;

Scene* AniamtionScene::createScene()
{
	auto scene = Scene::create();
	auto layer = AniamtionScene::create();
	scene->addChild(layer);

	return scene;
}

bool AniamtionScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	SpriteFrameCache::getInstance()->addSpriteFramesWithFile("grossiniani.plist");

	Vector<SpriteFrame*> animFrames(16);
	char szFileName[256] = { 0 };
	for (int i = 1; i < 15; i++) // 1~7 , callback , 8 ~ 14
	{
		sprintf(szFileName, "grossini_dance_%02d.png", i);
		SpriteFrame* frame = SpriteFrameCache::getInstance()->getSpriteFrameByName(szFileName);

		animFrames.pushBack(frame);
	}

	Sprite *pMan = Sprite::createWithSpriteFrameName("grossini_dance_01.png");
	pMan->setPosition(Vec2(240, 160));
	addChild(pMan);

	Animation* animation = Animation::createWithSpriteFrames(animFrames, 0.1f);
	Animate* animate = Animate::create(animation);
	pMan->runAction(RepeatForever::create(animate));

	return true;
}

void AniamtionScene::CallBackFunc()
{

}

