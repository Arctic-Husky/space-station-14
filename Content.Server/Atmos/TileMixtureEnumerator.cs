using System.Diagnostics.CodeAnalysis;
<<<<<<< HEAD
using Content.Shared.Atmos;
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

namespace Content.Server.Atmos;

public struct TileMixtureEnumerator
{
    public readonly TileAtmosphere?[] Tiles;
    public int Index = 0;

    public static readonly TileMixtureEnumerator Empty = new(Array.Empty<TileAtmosphere>());

    internal TileMixtureEnumerator(TileAtmosphere?[] tiles)
    {
        Tiles = tiles;
    }

    public bool MoveNext([NotNullWhen(true)] out GasMixture? mix)
    {
        while (Index < Tiles.Length)
        {
            mix = Tiles[Index++]?.Air;
            if (mix != null)
                return true;
        }

        mix = null;
        return false;
    }
}
