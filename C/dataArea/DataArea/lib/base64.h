#pragma once

#ifndef _BASE64_HH  
#define _BASE64_HH  

#ifdef __cplusplus  
extern "C" {
#endif  

    unsigned char* base64Decode(char* in, unsigned int* resultSize, int trimTrailingZeros);
    // returns a newly allocated array - of size "resultSize" - that  
    // the caller is responsible for free.  
    // trimTrailingZeros  default 1

    char* base64Encode(char const* orig, unsigned origLength);
    // returns a 0-terminated string that  
    // the caller is responsible for free.

    void base64Free(unsigned char * ptr);
    // the caller can use for free the memory of base64Decode or base64Encode.
#ifdef __cplusplus  
}
#endif  

#endif  
