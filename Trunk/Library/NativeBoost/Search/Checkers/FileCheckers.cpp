#include "stdafx.h"
#include "FileCheckers.h"
namespace search{
/***********************************************************************************/
bool FileCheckByNameExactMatch::Check(const tstring& text) const
{
	return name->Compare(text);
}
/***********************************************************************************/
bool FileCheckByNameBeginFrom::Check(const tstring& text) const
{
	if (name->Length() > text.length())
		return false;
	return name->Compare(text.substr(0, name->Length()));
}
/***********************************************************************************/
bool FileCheckByNameEnds::Check(const tstring& text) const
{
	if (name->Length() > text.length())
		return false;
	return name->Compare(text.substr(text.length() - name->Length(), name->Length()));
}
/***********************************************************************************/
bool FileCheckByNameContains::Check(const tstring& text) const
{
	if (name->Length() > text.length())
		return false;
	return name->ContainMe(text);
}
/***********************************************************************************/
}