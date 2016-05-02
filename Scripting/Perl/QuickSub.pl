#!/usr/bin/perl -w

while (<STDIN>) {
    chomp;
    print &FilterIncludeNumbers($_);
}

sub FilterIncludeNumbers {
    my $input = shift;
    if (/[0-9]/) {
        return sprintf("%s\n", $_);        
    }
}