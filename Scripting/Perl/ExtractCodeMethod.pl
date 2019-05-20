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

my @methodLines;
my @hashKeys = ("mode.Value.PrimaryChamber", "mode.Value.SecondaryChamber");

# Help sub... ;)
sub usage() {
  print("\nProgrammed by Ira\n");
  print("==========================\n");
  print("You may use this program under the terms of the GPL.\n");
  print("Feel free to modify this code in any way you want.\n\n");
  print("Usage  : ReadFile [options]\n");
  print("Options:        -f <file>, to read input from <file>\n");
  print(" "x16,"-m <method name> to indicate which method to extract\n");
  print(" "x16,"-s <search string> -c <count> to dump <count> lines AFTER encountering <search string>\n");
  print(" "x16,"-h or -? displays this help\n");
  print("\n");
  exit 0;
}

my %opt;
getopts('f:m:?s:c:h', \%opt)||die("Error: Incorrect command line options !!!\n");

# Checking if anyone needs help... ;) 
if (defined($opt{'?'}) || defined($opt{'h'})) {
  usage();
}
if (defined($opt{'f'}) && defined($opt{'m'})) {
  extractMethod($opt{'f'}, $opt{'m'});
}
elsif (defined($opt{'f'}) && defined($opt{'s'}) && defined($opt{'c'})) {
  extractLines($opt{'f'}, $opt{'s'}, $opt{'c'});
}
elsif (defined($opt{'f'})) {
  dumpAllSigs($opt{'f'});
}
elsif (defined($opt{'m'})) {
  extactMethodName($opt{'m'});
}
else {
  usage();
}

sub extactMethodName {
    $_ = shift;
    # if (/^\s+(?:static )?(?:private|public|internal|protected)+\s\w+\s([^(]+)\(.*/) {
    if (/^\s+(static )?(private|public|internal|protected)+.+/) {
        printf "%s", $1;
    }
    else {
        printf "NOK";    
    }
}

sub dumpAllSigs {
  my $file = shift;
  open(FH, "<$file")||die("Error: Cannot open file $file for input !!!\n\n");
  
  foreach (<FH>) {
    if (/^\s+(static )?(private|public|internal|protected)+.+/) {
        printf "%s", $_;
    }    
  }
}

sub extractMethod {
  my $file = shift;
  my $method_name = shift;
  open(FH, "<$file")||die("Error: Cannot open file $file for input !!!\n\n");
  my @methodLines;
  my $collecting_method_lines = 0;
  foreach (<FH>) {
    if (/^\s+(static )?(private|public|internal|protected)+.+$method_name/) {
        $collecting_method_lines = 1;
        push(@methodLines, $_);
    }
    elsif ($collecting_method_lines) {
        if (/^\s+(static )?(private|public|internal|protected)+.+/) {
            $collecting_method_lines = 0;
        }
        else {
            push(@methodLines, $_);
        }        
    }    
  }
  
  foreach my $methodLine (@methodLines) {
    printf "%s", $methodLine;
  }
}

sub extractLines {
  my $file = shift;
  my $searchStr = shift;
  my $linesCount = shift;
  my $decr = $linesCount;
  my $collecting_lines = 0;
  my $line = "";
  # printf "Extracting lines from '%s' using '%s' then '%s' lines\r\n", $file, $searchStr, $linesCount;
  open(FH, "<$file")||die("Error: Cannot open file $file for input !!!\n\n");
  foreach (<FH>) {
    $line = $_;
    if ($collecting_lines) {
        printf "%s", $line;
        $decr = --$decr;
    }
    if ($decr == 0) {
      $collecting_lines = 0;
      $decr = $linesCount;
    }
    # printf "index(...)=%s\r\n", index($line, $searchStr);
    if (index($line, $searchStr) != -1) {
        $collecting_lines = 1;
    }
    # printf "decr: %s\r\n", $decr;
  }
}
