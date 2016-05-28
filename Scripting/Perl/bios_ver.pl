#!\usr\bin\perl
#*******************************************************************************
#   INTEL CONFIDENTIAL
#   Copyright 2013
#   Intel Corporation All Rights Reserved.
#
# The source code contained or described herein and all documents
# related to the source code ("Material") are owned by Intel Corporation
# or its suppliers or licensors. Title to the Material remains with
# Intel Corporation or its suppliers and licensors. The Material may
# contain trade secrets and proprietary and confidential information
# of Intel Corporation and its suppliers and licensors, and is protected
# by worldwide copyright and trade secret laws and treaty provisions. No
# part of the Material may be used, copied, reproduced, modified,
# published, uploaded, posted, transmitted, distributed, or disclosed
# in any way without Intel’s prior express written permission.
# No license under any patent, copyright, trade secret or other
# intellectual property right is granted to or conferred upon you
# by disclosure or delivery of the Materials, either expressly, by
# implication, inducement, estoppel or otherwise. Any license under
# such intellectual property rights must be express and approved by
# Intel in writing.
#
#
# Unless otherwise agreed by Intel in writing, you may not remove
# or alter this notice or any other notice embedded in Materials by
# Intel or Intel’s suppliers or licensors in any way.
#******************************************************************************

#******************************************************************************
#
#  Program: bios_ver.pl
#
#  Abstract:
#     bios_ver.pl is a perl script that examines a binary 8K bios file and
#  extracts and displays its version number
#
#     It is written in perl since this scripting language seems to be common
#  on all Validation & Developments system's cygwin shells.
#
#******************************************************************************
#  Author:  dosheimx
#  Date:    01/16/2015
#******************************************************************************
#  Modification History:
#  =-=-=-=-=-=-=-=-=-=-=
#  20150116 dosheimx    Initial Creation.
#  20150121 dosheimx    Ver 0.2 Removed extranious spaces in output version string.
#  20150121 dosheimx    Ver 0.3 Renamed vars to be more generic (e.g. less BIOS centric)
#
#******************************************************************************

# Global Variables
$name             = $0;       # Script's name
$name             =~ s/.*\\//g;
$name             =~ s/\.pl//;
$ver              = "0.3";    # script version
$ver_date         = "01-21-2015 (dosheimx)";
$signon_msg       = sprintf("%s ver %s %s",
                     $name,
                     $ver,
                     $ver_date);

$debug            = 0;        # Turns on/off very verbose debug messages. Intended for script development only.
$ver_offset       = 0;        # number of character to version string.
$bin_file_found   = 0;
$bin_file         = "";       # Binary filename (with path prefix'ed to it)
# 
#******************************************************************************
#  === main ===
#  Outer most level of this script.
#******************************************************************************
main:
{

   # Display sign-on Message
   printf("%s ver %s %s\n",
         $name,
         $ver,
         $ver_date);

   # Get cmd line arguments
   get_args();
   
   # Verify the necessary zips, dirs & files are present.
   find_ver($bin_file);
   
   exit(0);
}

#******************************************************************************
#******************************************************************************
#
#  Proc: get_args
#
#  Abstract:
#     Looks at the argument passed on the command line, checks for correctness
#  and sets appropriate variables.
#
#  Global Vars Accessed:
#     $debug
#     $bin_file
#
#******************************************************************************
#******************************************************************************
sub get_args {
   printf("Debug: <get_args> Entering sub.\n") if ($debug>1);

   my $arg;
   my $param_count;
   my $bin_file_found = 0;
      
   $param_count = scalar (@ARGV);

   if ($param_count == 0) {
      &help_msg();
      exit(1);
   }

   if ( $debug > 0) {
      printf("Debug: Param count is %d\n",$param_count);
      for ($i=0;$i<$param_count;$i++)
      {
         printf("Debug: ARGV %d: \"%s\"\n",$i,$ARGV[$i])
      }
   }

   if ($param_count > 2) {
      printf("***ERROR: Too many command line arguments. (%d)\n\n",$param_count);
      &help_msg();
      exit(1);
   }

   for($i=0;$i<=(scalar @ARGV);$i++) {
      next if ($ARGV[$i] eq "");
      $arg = $ARGV[$i];
      printf("Debug: Processing ARGV[%d]: \"%s\"\n", $i, $arg) if ( $debug > 0);
      if ( $arg =~ /^-/ ) {
         printf("Debug: arg \"%s\" is a cmd line \"-\" option.\n", $arg) if ( $debug > 0);
         if ($arg =~ s/d//i) {
            # don't set to 1 if $debug is already turned on.
            if (! $debug) {
               printf("Turning debug ON.\n");
               $debug = 1;
            }
         }
         if (($arg =~ s/h//i) || ($arg =~ s/\?//)) {
            &help_msg();
            exit (0);
         }
         if ($arg ne "-"){
            printf("***ERROR: Undefined command line switch. Aborting. (\"%s\")",$arg);
            exit (1);
         } 
      } else {
         $bin_file_found = 1;
         $bin_file = $arg;
         # Convert Windows 'slashes' to Unix style 'slashes'...
         $bin_file =~ s/\\/\//g;
         $bin_file =~ s/\\\\/\/\//g;
         # Prepend "./" onto bin file, if no path present.
         if ($bin_file !~ m/\// ) {
            $bin_file =~ s/^/\.\//;
         }
      }
   }
   if ($bin_file_found == 0) {
      printf("***ERROR: Did not find .bin\/.rom file parameter. Aborting. (\"%s\")",$arg);
      exit (1);
   }

   printf("Debug: Version file: \"%s\"\n",$bin_file) if ($debug);
   printf("Debug: <get_args> Exiting sub.\n") if ($debug>1);
   ;

}    
#  End of sub "get_args"
#******************************************************************************


#******************************************************************************
#******************************************************************************
#
#  Proc: help_msg
#
#  Abstract:
#
#  Global Vars Accessed:
#
#******************************************************************************
#******************************************************************************
sub help_msg {
   printf("Debug: <help_msg> Entering sub.\n") if ($debug>1);

   printf("%s Help\n",$name);
   printf("\tThe intention of this script is to read a binary bios or image file\n");
   printf("\tand extract and display it's version number.\n\n");
   printf("\tThe path parameter can be either Windows format (with \"\\\")\n");
   printf("\tor Unix format (with \"/\").\n\n");
   printf("\tIMPORTANT: Use single quote (\"\'\") around windows paths.\n\n");
   printf("\t%s [-d | -v | -h] <path>\n", $name);
   printf("\t\t-d\tDebug\tTurns on debug messages. Usefil only for script debug.\n");
   printf("\t\t-h\tHelp\tPrints this message.\n");
   printf("\t\t<path>\tPath\tPath - ENDING WITH .bin\/.rom FILENAME.\n\n");
   printf("\tCommand Examples:\n");
   printf("\t\tperl %s.pl \'\\\\amr\\ec\\proj\\DPGEC\\iag\\dcg\\FW_EVAL\\...\\BIOS\\MCWBG.rom'\n", $name);
   printf("\t\tperl %s.pl -v image.bin\n", $name);
   printf("\t\tperl %s.pl -d GRRFCRB1.86B.0224.V01.1501080915_MAYAN_HCLK.rom\n\n",$name);

   printf("Debug: <help_msg> Exiting sub.\n") if ($debug>1);
}    
#  End of sub "get_args"
#******************************************************************************
   

#******************************************************************************
#******************************************************************************
#
#  Proc: find_ver
#
#  Abstract:
#
#  Passed Parameters:
#     $file       File name with full path to .bin / .rom file
#
#  Global Vars Accessed:
#     $debug
#     $verbose
#
#******************************************************************************
sub find_ver {
   
   printf("Debug: <find_ver> Entering sub.\n") if ($debug>1);

   # full path with filename passed as sole parameter
   my ($file) = @_;
   
   my $buffer = "";
   printf "%s is ", $file;
   open (BIN_FILE, "<", $file) or die "Not able to open the file. \n";
   printf("Debug: <find_ver> Opened binary file \"%s\".\n",$file) if ($debug);
   binmode (BIN_FILE);
   
   my $bytenum=0;
   my $tmp = "";

   my $version_string_found = 0;
   my $version_string_count = 0;
   my $version_string_length = 64;
   my $version_string = "";

   my $string_prefix = ("BIOSI\$");
   my $string_prefix_count = 0;
   my $string_prefix_found = 0;

   my @prefix = split("", $string_prefix);

   if ($debug) {
      printf("Debug: string_prefix: \"%s\"\n", "$string_prefix");
      for($i=0;$i<6;$i++) {
         printf("Debug: Prefix %s \"%c\"\n", $i, $prefix[$i]);
      }
   }
   #return;
   while ((read (BIN_FILE, $buffer, 65536)) != 0 ) {
      my @data=split(//,$buffer);
      foreach $b (@data)
      {
         if ((0) && ($debug) && ($bytenum >= 0) && ($bytenum < 256)){
            if (($bytenum % 16) == 0) {
               printf("%06X ",$bytenum)
            }
            printf("0x%02x ", ord($b));
            if (($bytenum % 16) == 15) {
               printf("\n")
            }
         }

         # Capture the version string 
         $j = chr(ord($b));
         if (( $string_prefix_found == 1 ) 
         &&  ( $version_string_found == 0 )
         &&  ( $version_string_count < $version_string_length )) {
            $version_string .= sprintf("%s", $j );
            $version_string_count++;
            #printf("%09d %d: \"%s\" \"%s\"\n", $bytenum, $version_string_count, $j, $version_string );
         } elsif (($version_string_count == $version_string_length)
         &&  ($version_string_found == 0 )) {
            $version_string_found = 1;
            $version_string =~ s/[^a-zA-Z0-9\.]//g;
            printf("Ver: %s\n", $version_string);
         }

         # Find the string prefix heading 
         if (($string_prefix_found == 0) && (chr(ord($b)) eq $prefix[$string_prefix_count])) {
            $string_prefix_count++;
            #printf("%09d Found prefix char: \"%c\"\n", $bytenum, ord($b)) if ($debug);
         } else {
            $string_prefix_count = 0;
         }
         if (($string_prefix_count == 6) && ($string_prefix_found == 0)) {
            $string_prefix_found = 1;
            $version_string_count = 0;
            $version_string_found = 0;
            printf("%06X Found: \"%s\" \!\!\!\n",$bytenum,$string_prefix) if ($debug);
         }

         $bytenum++
      }  
   }
   close(BIN_FILE);
   printf("Debug: <find_ver> Closed version file \"%s\".\n",$file) if ($debug);

   printf("Debug: <find_ver> Exiting sub.\n") if ($debug>1);
}
#  End of sub "find_ver"
#******************************************************************************

#******************************************************************************
#  End of "bios_ver.pl" file
#******************************************************************************
