# Auto-generated from csapi-flux.spec.in by makespec.sh

%define FLUX_RPM_VERSION 0.0.1.18183
%define FLUX_ASSEMBLY_PATH /usr/share/dotnet.tizen/framework
%define FLUX_PATH src/Tizen.NUI.FLUX/Tizen.NUI.FLUX.csproj
%define FLUX_COMPONENT_PATH src/Tizen.NUI.FLUX.Component/Tizen.NUI.FLUX.Component.csproj

Name:       csapi-flux
Summary:    Assemblies of Tizen NUI FLUX
Version:    %{FLUX_RPM_VERSION}
Release:    1
Group:      Development/Libraries
License:    samsung
URL:        https://www.tizen.org
Source0:    %{name}-%{version}.tar.gz
Source1:    %{name}.manifest

BuildArch:   noarch
AutoReqProv: no

BuildRequires: dotnet-build-tools
Requires(post): /usr/bin/vconftool

%description
%{summary}

%prep
%setup -q
cp %{SOURCE1} .

%build
dotnet restore %{FLUX_PATH} -s packaging/depends
dotnet build --no-restore %{FLUX_PATH}
dotnet restore %{FLUX_COMPONENT_PATH} -s packaging/depends
dotnet build --no-restore %{FLUX_COMPONENT_PATH}

%install
mkdir -p %{buildroot}%{FLUX_ASSEMBLY_PATH}
install -p -m 644 src/Tizen.NUI.FLUX.Component/bin/Debug/netstandard2.0/*.dll %{buildroot}%{FLUX_ASSEMBLY_PATH}

%post
/usr/bin/vconftool set -t string db/dotnet/flux_path %{FLUX_ASSEMBLY_PATH} -f

%files
%license LICENSE.samsung
%manifest %{name}.manifest
%{FLUX_ASSEMBLY_PATH}/*.dll

%clean
rm -rf %{buildroot}
