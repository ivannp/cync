#ifndef DLL_API_H
#define DLL_API_H

#ifdef _MSC_VER
#ifdef DLL_API_EXPORTS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif // DLL_API_EXPORTS
#else
#define DLL_API
#endif // _MSC_VER

#endif // DLL_API_H