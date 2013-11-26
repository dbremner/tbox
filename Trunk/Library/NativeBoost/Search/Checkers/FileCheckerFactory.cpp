#include "stdafx.h"
#include "FileCheckerFactory.h"
namespace search
{
/***********************************************************************************/
IName* FileCheckerFactory::GetNameComparer(const tstring& name, bool needMathCase)
{
	if (needMathCase) return new NameCaseComparer(name);
	return new NameNoCaseComparer(name);
}
/***********************************************************************************/
IFileChecker* FileCheckerFactory::Create(CompareType compareType, bool needMathCase, const tstring& name)
{
	IName* nameComparer = GetNameComparer(name, needMathCase);
	switch (compareType)
	{
	case ExactMath:
		return new FileCheckByNameExactMatch(*nameComparer);
	case BeginWith:
		return new FileCheckByNameBeginFrom(*nameComparer);
	case EndWith:
		return new FileCheckByNameEnds(*nameComparer);
	case Contain:
		return new FileCheckByNameContains(*nameComparer);
	default:
		throw tstring(L"Searcher. Unknown file checker");
	}
}
/***********************************************************************************/
}