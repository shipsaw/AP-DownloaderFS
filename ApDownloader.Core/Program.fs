module Project

open System.Net
open System.Windows
open Elmish.WPF
open Elmish
open Domain
open DataAccess

/// This is the main data model for our application
type Model =
    { Busy: bool
      AllApProducts: Product seq
      PurchasedProducts: Product list
      DownloadedProducts: Product seq
      DiskProducts: Product seq
      ListItemProducts: ProductListItem seq
      ClientCookies: CookieContainer option
      UserEmail: string
      DlConfig: DownloadConfig }

type MessageType =
    | Exit
    | Login of obj
    | PrepareDownloadView
    | EmailChange of string
/// Commands
let ExitCommand (dispatch: MessageType -> unit) : unit = Application.Current.Shutdown()
/// This is used to define the initial state of our application
let init () =
    { Busy = false
      AllApProducts = GetAllProductsOption
      PurchasedProducts = List.empty
      DownloadedProducts = Seq.empty
      DiskProducts = Seq.empty
      ListItemProducts = Seq.empty
      ClientCookies = None
      UserEmail = ""
      DlConfig =
          { GetExtraStock = true
            GetBrandingPatch = true
            GetLiveryPack = true
            DownloadFilepath = @"C:\"
            InstallFilepath = @"C:\" } },
    Cmd.none

/// This is a discriminated union of the available messages from the user interface
let ProductToListItem (product: Product) =
    { ProductId = product.ProductId
      ImageUrl = product.ImageName
      Name = product.Name
      CanUpdate = false
      IsNotOnDisk = false }

/// This is the Reducer Elmish.WPF calls to generate a new model based on a message and an old model
let update (msg: MessageType) (model: Model) =
    match msg with
    | Exit -> model, Cmd.ofSub ExitCommand
    | Login userPw -> { model with ClientCookies = HttpAccess.AttemptLogin(model.UserEmail, userPw) }, Cmd.none
    | EmailChange email -> { model with UserEmail = email }, Cmd.none
    | PrepareDownloadView -> model, Cmd.none

/// Elmish uses this to provide the data context for your view based on a model
let bindings () : Binding<Model, MessageType> list =
    [
      // One-Way Bindings
      //"ListItemProducts"
      "IsBusy" |> Binding.oneWay (fun m -> m.Busy)
      "ListItemProducts"
      |> Binding.oneWaySeqLazy ((fun m -> m.AllApProducts), refEq, id, (=), (fun item -> item.Name))
      "Exit" |> Binding.cmd Exit
      "DownloadViewCommand"
      |> Binding.cmd PrepareDownloadView
      "EmailChange"
      |> Binding.twoWay ((fun m -> m.UserEmail), (string >> EmailChange))
      "Login" |> Binding.cmdParam (fun pw -> Login pw)
      //"OptionsViewCommand" |> Binding.cmd OptionsView ]
      ]

/// This is the application's entry point. It hands things off to Elmish.WPF
let main window =
    let program =
        Program.mkProgramWpf init update bindings
    //|> Program.withLogger (new SerilogLoggerFactory(logger))
    Program.startElmishLoop ElmConfig.Default window program
