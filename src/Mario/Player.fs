module Player

type Size =
  | Small
  | Large

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
  | { size = Size.Large } ->
    { player with size = Size.Small; bag = Some(Mushroom) }
  | { bag = Some _ } ->
    { player with bag = None}
  | _ ->
    { player with hearts = player.hearts - (Lifes 1) }

let pickupMushroom player =
  match player with
  | { size = Size.Large } ->
    player
  | _ ->
    { player with bag = Some Mushroom }

let findLife player =
  { player with hearts = player.hearts + (Lifes 1) }

let findFireFlower player =
  { player with size = Size.Large; bag = None }
