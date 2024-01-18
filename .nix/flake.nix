{
  description = "";

  inputs.flake-utils.url = "github:numtide/flake-utils";

  outputs = { self, nixpkgs, flake-utils }:
    flake-utils.lib.eachDefaultSystem
      (system:
        let pkgs = nixpkgs.legacyPackages.${system}; in
        {
          devShells.default =
            let
              pkgs = import nixpkgs {
                inherit system;
              };
            in
            pkgs.mkShell {
              packages = with pkgs; [
                # development environment
                dotnetCorePackages.sdk_6_0
                mono
                # running environment
                dotnet-runtime
                libglvnd
                udev
                alsa-lib
                xorg.libXi
              ];

              shellHook = ''e
                export LD_LIBRARY_PATH="/run/opengl-driver/lib/:${pkgs.lib.makeLibraryPath [pkgs.libGL pkgs.udev pkgs.xorg.libXi pkgs.alsa-lib]}";
                echo "Ready with dotnet `${pkgs.dotnetCorePackages.sdk_6_0}/bin/dotnet --version`";
              '';
            };
        }
      );
}
