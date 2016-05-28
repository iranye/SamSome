###
# Read and output updated file test.
#
# Sample Input file contents:
# <div id="Div1">
# <a href="SamAndKatyB.html">Previous</a>
# <a href="SamAndKatyD.html">Next</a>
# </div>
###
use strict; use warnings;
use Getopt::Std;

# Help sub... ;)
sub usage() {
  print("\nPProgrammed by Ira\n");
  print("==========================\n");
  print("You may use this program under the terms of the GPL.\n");
  print("Feel free to modify this code in any way you want.\n\n");
  print("Usage  : UpdateFile [options]\n");
  print("Options:        -f <file>, to read input from <file>\n");
  print(" "x16,"-h or -? displays this help\n");
  print("\n");
  exit 0;
}

my %opt;
getopts('f:?h', \%opt)||die("Error: Incorrect command line options !!!\n");

# Checking if anyone needs help... ;) 
if (defined($opt{'?'}) || defined($opt{'h'})) {
  usage();
}
# Test if someone specified -f <file> and open it (perhabs...) ;)
if (defined($opt{'f'})) {
  readAndReplace($opt{'f'});
}

sub sub_test {
  # my $arg = shift;
  
  # foreach my $key (@hashKeys) {
    # print "key: $key\n";
  # }
  # my @arr = ("foo");
  # print "@arr\n";
  # push(@arr, "bar");
  # print "@arr\n";

  # my $hash_ref1 = { 1=> 1 };
  # my $hash_ref2 = $hash_ref1;
  
  # print "hash_ref1 $hash_ref1\n";
  # print "hash_ref2 $hash_ref2\n"; # same, same

  # my %hash;
  # $hash{"7"} = "sieben";
  # my $size = scalar(keys %hash);
  # printf "hash size: %d\n", $size;
  # %hash = undef;
  # printf "hash size: %d\n", scalar(keys %hash);
}

sub readAndReplace {
  # sub_test();
  
  my $file = shift;

  open(FH, "<$file")||die("Error: Cannot open file $file for input !!!\n\n");
  
  my $fileOrdChar = getfileOrdChar($file);
  
  while(<FH>) {
    chomp;
  
    if (/[^"]+["]([\w]+)([A-Z]).html["][>]([\w]+)/) {
      printf "%s\n", getUpdatedHref($fileOrdChar, $1, $2, $3);
      # printf "%s\t%s\t%s\n", $1, $2, $3
    }
    else {
      print "$_\n";
    }
  }
}

sub getfileOrdChar {
  my $fileName = shift;
  if ($fileName =~ m/([\w]+)(\w)[.]/) {
    return $2;
  }
  return 0;
}

sub getUpdatedHref {
  my $fileOrdChar = shift;
  my $baseName = shift;
  my $index = shift;
  my $direction = shift;
  
  my $newOrdChar;
  if ($direction =~ m/Previous/) {
    $newOrdChar = getPreviousOrdChar($fileOrdChar);
  }
  elsif ($direction =~ m/Next/) {
    $newOrdChar = getNextOrdChar($fileOrdChar);
  }
  else {
    die("Error: failed to find 'Previous' or 'Next' within $direction\n");
  }
  # printf "%s => %s\n", $fileOrdChar, $newOrdChar;
  return sprintf "<a href=\"%s%s.html\">%s</a>", $baseName, $newOrdChar, $direction;
}

sub getPreviousOrdChar {
  my $ordVal = ord(shift) - 1;
  # print "previous $ordVal\n";
  return chr($ordVal);
}

sub getNextOrdChar {
  my $ordVal = ord(shift) + 1;
  # print "next $ordVal\n";
  return chr($ordVal);
}


