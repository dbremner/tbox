#pragma once
#include "..\..\Common.h"
namespace search{
enum CompareType
{
	ExactMath = 0,
	BeginWith = 1,
	EndWith = 2,
	Contain = 3,
};

class IFileChecker
{
public:
	virtual bool Check(const tstring& text) const = 0;
};

class IName
{
public:
	virtual bool Compare(const tstring& name) const = 0;
	virtual bool ContainMe(const tstring& name) const = 0;
	virtual tstring::size_type Length() const = 0;
};
}