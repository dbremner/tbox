#pragma once
#include "..\..\..\Common.h"
namespace search{
class IAdder
{
public:
	virtual void AddWord(const tstring& word, tstring::size_type fileId)=0;
};

struct AddInfo
{
	tstring Path;
	tstring::size_type Id;
};

class IParser
{
public:
	virtual bool Parse(const AddInfo& info)=0;
};

class Parser : public IParser
{
public:
	bool Parse(const AddInfo& info);
};
}