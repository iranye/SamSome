###
# Read and parse file info where info is grouped in lines of three, e.g.,:
# mode.Key: ODO
# mode.Value.PrimaryChamber: Ventricle
# mode.Value.SecondaryChamber: Atrium
# mode.Key: AOO
# mode.Value.PrimaryChamber: Atrium
# mode.Value.SecondaryChamber: NotApplicable
###
use strict; use warnings;
use Getopt::Std;

my @modeInfos;
my @hashKeys = ("mode.Value.PrimaryChamber", "mode.Value.SecondaryChamber");

# Help sub... ;)
sub usage() {
  print("\nProgrammed by Ira\n");
  print("==========================\n");
  print("You may use this program under the terms of the GPL.\n");
  print("Feel free to modify this code in any way you want.\n\n");
  print("Usage  : ReadFile [options]\n");
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
  parseFile($opt{'f'});
}

sub sub_test() {
  foreach my $key (@hashKeys) {
    print "key: $key\n";
  }
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

sub parseFile {
  # sub_test();
  
  my $file = shift;  
  open(FH, "<$file")||die("Error: Cannot open file $file for input !!!\n\n");
  my $lines_per_block = 3;
  my @modeInfoLines;
  my $i = 1;
  foreach (<FH>) {
    chomp();
    push(@modeInfoLines, $_);
    if ($i++ == $lines_per_block) {
      $i = 1;
      parseModeInfo(@modeInfoLines);
      undef(@modeInfoLines);
    }
  }
  printModeInfo();
}

sub printModeInfo {
  printf "%s\t%s\t%s\t\n", "mode", $hashKeys[0], $hashKeys[1];
  foreach my $modeInfo (@modeInfos) {
    printf "%s\t%s\t%s\n", $$modeInfo{"mode"}, $$modeInfo{$hashKeys[0]}, $$modeInfo{$hashKeys[1]};
  }
}
sub parseModeInfo {  
  print "parseModeInfo\n";
  
  my %modeInfo;
  foreach (@_) {
    if (/([^:]+):[\s]*(.*)/) {
      if ($1 eq "mode.Key") {
        $modeInfo{"mode"} = $2;
      }
      else {
        $modeInfo{$1} = $2;
      }
    }
  
    else {
      die("Error: bad format in the line: '$_' !!!\n\n");    
    }
  }
  push(@modeInfos, \%modeInfo);}














