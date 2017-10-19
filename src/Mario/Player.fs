module Player

type Size =
  | Small

type Bag =
  | Mushroom

type Hearts =
  | Dead
  | Lifes of int
  with
  static member (-) (lhs:Hearts, rhs:Hearts) =
    match (lhs, rhs) with
    | (Lifes l, Lifes r) when (l - r) > 0 ->
      Lifes (l - r)
    | _ -> Dead
  static member (+) (lhs:Hearts, rhs:Hearts) =
      match (lhs, rhs) with
      | (Lifes l, Lifes r) ->
        Lifes (l + r)
      | (Dead _, Lifes r) ->
        Lifes r
      | _ ->
        Dead

type SuperMario =
  {
    hearts : Hearts
    size : Size
    bag : Bag option
  }

let player =
  {
    hearts = Lifes 3
    size = Small
    bag = None
  }

let hit player =
  match player with
  | { bag = Some _ } ->
    { player with bag = None}
  | _ ->
    { player with hearts = player.hearts - (Lifes 1) }

let pickupMushroom player =
  { player with bag = Some Mushroom }

let findLife player =
  { player with hearts = player.hearts + (Lifes 1) }
