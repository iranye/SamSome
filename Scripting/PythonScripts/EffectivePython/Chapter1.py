import os

def write_binary():
    with open('random.bin', 'wb') as f:
        f.write(os.urandom(10))
        
def parse_query():
    from urllib.parse import parse_qs
    my_values = parse_qs('red=5&blue=0&green=', keep_blank_values=True)
    print(repr(my_values))

if __name__ == "__main__":
    # write_binary()
    parse_query()