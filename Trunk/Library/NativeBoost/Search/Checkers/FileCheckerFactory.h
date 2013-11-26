#pragma once
#include "FileCheckers.h"
#include "NameComparers.h"

namespace search{

class FileCheckerFactory
{
public:
	IName* GetNameComparer(const tstring& name, bool needMathCase);
	IFileChecker* Create(CompareType compareType, bool needMathCase, const tstring& name);
};

}