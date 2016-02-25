#!/usr/bin/perl -w
use strict;

my @listings;
#  print "   <ul>\n";
while(<>) {
    #print;
    chomp;
      #write('');        write("<BR>");

    # s/^ {4}//;
    s/\$field_names\{([^}]+)\}\s*=\s*\"([^"]+)\";/\'$1\' => \'$2\',/;
    printf("%s\n", $_);
    push(@listings, $_);
}

# use this loop to correctly space out assignment statements
foreach my $listing (@listings) {
    my $line = $listing;
    my $max = 0;
    $line =~ s/(.*)(=>).*/$1/;
    # print $line, "\n";

}
#$field_names{username}    = "Login Name";
# %account = ('username' => 'jimmyboy', 'password' => 'scar3me');

#$field_names{agent_name}  = "Agent First and Last Name";

