#include "iostream" // less than and greater than are disabled
int main(int argc, char** argv)
{
    unsigned int sum = 0;
    std::cout << sum;
	int i = 1;
    for(int i = 1; i < 1001; ++i)
        if(!(i % 3) && !(i % 5))
            sum += i;
    std::cout << sum;
    return 0;
}
