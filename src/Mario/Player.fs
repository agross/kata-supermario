module Player

type Size =
  | Small

type Bag =
  | Mushroom

type SuperMario =
  {
    alive : bool
    size : Size
    bag : Bag option
  }

let player =
  {
    alive = true
    size = Size.Small
    bag = None
  }

let hit player =
  match player with
  | { bag = Some _ } -> { player with bag = None}
  | _ -> { player with alive = false }

let pickupMushroom player =
  { player with bag = Some Bag.Mushroom }
