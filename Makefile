#
# Makefile to compile RunUO for mono
# adapted from Fyzxs setup script
# http://www.runuo.com/community/threads/518599/
#
LIBZ = libz.so
RUNTIME = mono-csc
RUNTIME_FLAGS = -out:RunUO.exe		\
	-d:MONO				\
	-optimize+			\
	-unsafe				\
	-r:System			\
	-r:System.Configuration.Install	\
	-r:System.Data			\
	-r:System.Drawing		\
	-r:System.EnterpriseServices	\
	-r:System.Management		\
	-r:System.Security		\
	-r:System.ServiceProcess	\
	-r:System.Web			\
	-r:System.Web.Services		\
	-r:System.Windows.Forms		\
	-r:System.Xml			\
	-nowarn:219			\
	-recurse:Server/*.cs

all:
	$(RUNTIME) $(RUNTIME_FLAGS)
	echo "<configuration>" > RunUO.exe.config
	echo "	<dllmap dll=\"libz\" target=\"$(LIBZ)\" />" >> RunUO.exe.config
	echo "</configuration>" >> RunUO.exe.config

clean:
	rm -rf RunUO.exe
	rm -rf RunUO.exe.config
