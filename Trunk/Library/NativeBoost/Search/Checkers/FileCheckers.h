#pragma once
#include "Interfaces.h"
namespace search{
class FileCheckByNameExactMatch : public IFileChecker
{
public: 
	FileCheckByNameExactMatch(IName& name):name(&name){}
	bool Check(const tstring& text) const;
private:
	const IName* name;
};

class FileCheckByNameBeginFrom : public IFileChecker
{
public: 
	FileCheckByNameBeginFrom(IName& name):name(&name){}
	bool Check(const tstring& text) const;
private:
	const IName* name;
};

class FileCheckByNameEnds : public IFileChecker
{
public: 
	FileCheckByNameEnds(IName& name):name(&name){}
	bool Check(const tstring& text) const;
private:
	const IName* name;
};

class FileCheckByNameContains : public IFileChecker
{
public: 
	FileCheckByNameContains(IName& name):name(&name){}
	bool Check(const tstring& text) const;
private:
	const IName* name;
};

}