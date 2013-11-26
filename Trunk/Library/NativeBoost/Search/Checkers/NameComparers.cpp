#include "stdafx.h"
#include "NameComparers.h"
namespace search
{
/***********************************************************************************/
bool NameCaseComparer::Compare(const tstring& name) const
{
	return boost::equals(this->name, name);
}

bool NameCaseComparer::ContainMe(const tstring& name)const
{
	return boost::find_first(this->name, name) != nullptr;
}

tstring::size_type NameCaseComparer::Length() const
{
	return name.length();
}
/***********************************************************************************/
bool NameNoCaseComparer::Compare(const tstring& name) const
{
	return boost::iequals(this->name, name);
}

bool NameNoCaseComparer::ContainMe(const tstring& name)const
{
	return boost::ifind_first(this->name, name) != nullptr;
}

tstring::size_type NameNoCaseComparer::Length() const
{
	return name.length();
}
/***********************************************************************************/
}