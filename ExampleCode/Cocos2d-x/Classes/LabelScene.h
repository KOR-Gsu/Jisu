#ifndef __LABELSCENE_SCENE_H__
#define __LABELSCENE_SCENE_H__

#include "cocos2d.h"

class LabelScene : public cocos2d::Layer
{
public:
	static cocos2d::Scene* createScene();

	virtual bool init();

	CREATE_FUNC(LabelScene);
};

#endif

