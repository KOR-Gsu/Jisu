#include "LabelScene.h"
#include "SimpleAudioEngine.h"

USING_NS_CC;

Scene* LabelScene::createScene()
{
	auto scene = Scene::create();
	auto layer = LabelScene::create();
	scene->addChild(layer);

	return scene;
}

bool LabelScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	LabelTTF* pLabel = LabelTTF::create("���ع��� ��λ��� ������ ����� �ϴ����� �����ϻ� �츮���󸸼�", 
		"fonts/arial.ttf", 30, Size(300, 300));
	pLabel->setPosition(Vec2(240, 100));
	pLabel->setColor(Color3B(0, 255, 0));
	addChild(pLabel);


	LabelBMFont* pLabelBM = LabelBMFont::create("hello", "futura-48.fnt");
	pLabelBM->setPosition(Vec2(240, 20));
	addChild(pLabelBM);

	return true;
}

