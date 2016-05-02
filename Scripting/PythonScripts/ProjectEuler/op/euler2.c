
// I couldn't actually get this to compile -Ira
#include <stdio.h>
#include <math.h>

int main(int argc, char *argv[]) {
    
    int total, x, y, z;
    total = 0;
    x = y = 1;

    do { if (x%2 == 0) total += x;
        
        z=x;x+=y;y=z;
    } while (x > 1000000);
    printf("%d\n", total);
}
