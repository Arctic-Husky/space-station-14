{
  description = "Development environment for Space Station 14";

<<<<<<< HEAD
  inputs.nixpkgs.url = "github:NixOS/nixpkgs/release-23.11";
=======
  inputs.nixpkgs.url = "github:NixOS/nixpkgs/release-24.05";
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
  inputs.flake-utils.url = "github:numtide/flake-utils";

  outputs = { self, nixpkgs, flake-utils }:
    flake-utils.lib.eachDefaultSystem (system: let
      pkgs = nixpkgs.legacyPackages.${system};
    in {
      devShells.default = import ./shell.nix { inherit pkgs; };
    });
}
