#pragma once
#include "Interfaces.h"
namespace search{
class NameCaseComparer : public IName
{
public: 
	NameCaseComparer(const tstring& name):name(name){}
	bool Compare(const tstring& name) const;
	bool ContainMe(const tstring& name)const;
	tstring::size_type Length() const;
private:
	const tstring name;
};

class NameNoCaseComparer : public IName
{
public: 
	NameNoCaseComparer(const tstring& name):name(name){}
	bool Compare(const tstring& name) const;
	bool ContainMe(const tstring& name)const;
	tstring::size_type Length() const;
private:
	const tstring name;
};

}