###
# Update FW config files
#
###
use strict; use warnings;
use Getopt::Std;

# Help sub... ;)
sub usage() {
  print("\nProgrammed by Ira Nye\n");
  print("==========================\n");
  print("This script should run in the FW Config directory.  It makes new configs using the selected one\n");
  print("Usage  : perl CreateConfigs.pl -f <fw config file> -p [platform]\n");
  print("Options:        -f <file>, to read input from <file>\n");
  print(" "x16,"-p specifies the platform (e.g., GVP, GR, GVL, etc.\n");
  print(" "x16,"-h or -? displays this help\n");
  print("\n");
}

&main();

sub main() {
  my %opt;
  getopts('f:?p:?h', \%opt)||die("Error: Incorrect command line options !!!\n");

  # Checking if anyone needs help... ;) 
  if (defined($opt{'?'}) || defined($opt{'h'})) {
    usage();
    return;
  }
  my $base_fn;
  if (defined($opt{'f'})) {
    $base_fn = $opt{'f'};
  }
  else {
    usage();
    die "Please specify a FW config filename\n\n";
  }
  my $platform;
  if (defined($opt{'p'})) {
    $platform = $opt{'p'};
  }
  else {
    $platform = "GR";
  }

  printf("platform: '%s'\n", $platform);
  $base_fn =~ s/.xml//;
                    # <SEARCH_ELEMENT>, <OLD_TEXT>, <NEW_TEXT>, <REPLACE IN CURRENT LINE>
  my $mesdc_on = ["0x00", "0x38", 0];
  my $ptu_optin = ['Opt Out', 'Opt In - Option ROM enabled on demand', 1];
  my $generic_true_to_false = ["true", "false", 0 ];
  my $generic_false_to_true = ["false", "true", 0 ];
  my $switch_val_to_38 = ["0xE9", "0x38", 0];
  my $switch_1_to_0 = ["1", "0", 0];
  my $switch_1_to_98 = ["1", "98", 0];
  my $clock_mode;
  if ($platform eq "GVP") {
    $clock_mode  = ["Native Integrated Clock Control", "Native Integrated Clock Control", 0];
  }
  else {
    $clock_mode  = ["Native Integrated Clock Control", "Hybrid Clock Mode", 0];
  }
  
  
  my $ptuOD = {
    "MeMdesEn" => $generic_false_to_true,
    "MeMdesAddr" => $mesdc_on,
    "IccClockMode" => $clock_mode,
    "PTU Manufacturing Optin" => $ptu_optin,
    "PtuPayloadDisabled" => $generic_true_to_false,
  };
  my $ptu38 = {
    "MeMdesEn" => $generic_false_to_true,
    "MeMdesAddr" => $mesdc_on,
    "IccClockMode" => $clock_mode,
    "PTU Manufacturing Optin" => $ptu_optin,
    "PtuPayloadDisabled" => $generic_true_to_false,
    "OEM Platform Power Characterization Notification" => $switch_val_to_38,
  };
  my $ptuAW = {
    "MeMdesEn" => $generic_false_to_true,
    "MeMdesAddr" => $mesdc_on,
    "IccClockMode" => $clock_mode,
    "PTU Manufacturing Optin" => $ptu_optin,
    "PtuPayloadDisabled" => $generic_true_to_false,
  };
  my $ptuND = {
    "MeMdesEn" => $generic_false_to_true,
    "MeMdesAddr" => $mesdc_on,
    "IccClockMode" => $clock_mode,
  };
  my $ptuOO = {
    "MeMdesEn" => $generic_false_to_true,
    "MeMdesAddr" => $mesdc_on,
    "IccClockMode" => $clock_mode,
    "PtuPayloadDisabled" => $generic_true_to_false,
  };
  my $ptuPH = {
    "MeMdesEn" => $generic_false_to_true,
    "MeMdesAddr" => $mesdc_on,
    "IccClockMode" => $clock_mode,
    "PTU Manufacturing Optin" => $ptu_optin,
    "PtuPayloadDisabled" => $generic_true_to_false,
    "IgnorePROCHOT" => $generic_false_to_true,
  };
  my $cupsEnabledClfh = {
    "MeMdesEn" => $generic_false_to_true,
    "MeMdesAddr" => $mesdc_on,
    "IccClockMode" => $clock_mode,
    "CupsDisabled" => $generic_true_to_false,
    "StaticCoreLoadFactor" => $switch_1_to_98,
  };
  my $cupsEnabledMlfh = {
    "MeMdesEn" => $generic_false_to_true,
    "MeMdesAddr" => $mesdc_on,
    "IccClockMode" => $clock_mode,
    "CupsDisabled" => $generic_true_to_false,
    "StaticMemLoadFactor" => $switch_1_to_98,
  };
  my $cupsEnabledIlfh = {
    "MeMdesEn" => $generic_false_to_true,
    "MeMdesAddr" => $mesdc_on,
    "IccClockMode" => $clock_mode,
    "CupsDisabled" => $generic_true_to_false,
    "StaticIioLoadFactor" => $switch_1_to_98,
  };
  my $cupsDisabledSd = {
    "MeMdesEn" => $generic_false_to_true,
    "MeMdesAddr" => $mesdc_on,
    "IccClockMode" => $clock_mode,
    "CupsDisabled" => $generic_true_to_false,
    "CupsEnabled" => $switch_1_to_0,
  };
  my $cupsDisabledHd = {
    "MeMdesEn" => $generic_false_to_true,
    "MeMdesAddr" => $mesdc_on,
    "IccClockMode" => $clock_mode,
    "CupsDisabled" => $generic_false_to_true,
  };

  createNewConfig("$base_fn.xml", sprintf("$base_fn%s.xml", "_NatClockMesdcEnCupsDisHd"), $cupsDisabledHd);
  
  if ($base_fn =~ /_NM/) {
    createNewConfig("$base_fn.xml", sprintf("$base_fn%s.xml", "_PtuOD"), $ptuOD);
    createNewConfig("$base_fn.xml", sprintf("$base_fn%s.xml", "_Ptu38"), $ptu38);
    createNewConfig("$base_fn.xml", sprintf("$base_fn%s.xml", "_PtuAW"), $ptuAW);
    createNewConfig("$base_fn.xml", sprintf("$base_fn%s.xml", "_PtuND"), $ptuND);
    createNewConfig("$base_fn.xml", sprintf("$base_fn%s.xml", "_PtuOO"), $ptuOO);
    createNewConfig("$base_fn.xml", sprintf("$base_fn%s.xml", "_PtuPH"), $ptuPH);
  }
}

# Read config and create new based on supplied criteria
sub createNewConfig {
  my $in_file = shift;
  my $out_file = shift;
  my $hash_ref = shift;
  printf "read '%s' and save to '%s'\r\n", $in_file, $out_file;
  
  my %repl_construct = %{$hash_ref};
  # foreach (keys %repl_construct) {
    # printf "search_element: %s\n", $_;
  # }
  open(IFH, "<$in_file")||die("Error: Cannot open input file '$in_file' for input !!!\n\n");
  open(OFH, ">$out_file")||die("Error: Cannot open output file '$out_file' for output !!!\n\n");

  my $old_value;
  my $new_value;
  my $next_line;
  my $replace_in;
  
  while(<IFH>) {
    my $file_line = $_;

    if ($replace_in)
    {
      printf "Replacing '%s' with '%s'\n", $old_value, $new_value;
      $file_line =~ s/$old_value/$new_value/;
      $replace_in = 0;
    }
    foreach (keys %repl_construct) {
      if ($file_line =~ /$_/) {        my @repl_arr = @{$repl_construct{$_}};
        
        $old_value = $repl_arr[0];
        $new_value = $repl_arr[1];
        $next_line = $repl_arr[2];
        if ($next_line) {
          $replace_in = 1;
        }
        else {
          printf "Replacing [%s]: '%s' with '%s'\n", $_, $old_value, $new_value;
          $file_line =~ s/$old_value/$new_value/;
        }
      }
    }
    
    print OFH $file_line;
  }
  print "\n";
  close IFH;
  close OFH;
}
