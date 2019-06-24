#ifndef __MEMORY_SCENE_SCENE_H__
#define __MEMORY_SCENE_SCENE_H__

#include "cocos2d.h"

class MemoryScene : public cocos2d::Layer
{
private:
	cocos2d::Sprite* m_pSprite;
public:
	static cocos2d::Scene* createScene();

	virtual bool init();

	CREATE_FUNC(MemoryScene);
};

#endif

