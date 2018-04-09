module Player

open System

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
    immortalUntil : DateTime option
  }

let player =
  {
    hearts = Lifes 3
    size = Small
    bag = None
    immortalUntil = None
  }

type Hit = DateTime -> SuperMario -> SuperMario

let hit : Hit =
  fun hitTime player->
    match player with
    | { immortalUntil = Some until } when until > hitTime  ->
      player
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

let findStar (timeFound : DateTime) player =
  { player with immortalUntil = Some(timeFound.AddSeconds 2.) }
