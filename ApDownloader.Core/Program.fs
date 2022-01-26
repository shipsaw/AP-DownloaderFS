module Project

open System.Windows
open Elmish.WPF
open Elmish
open Domain
open System.Net.Http
open DataAccess

/// This is the main data model for our application
type Model =
    { Busy: bool
      AllApProducts: Product seq
      PurchasedProducts: Product list
      DownloadedProducts: Product seq
      DiskProducts: Product seq
      ListItemProducts: ProductListItem seq
      Client: HttpClient option
      DlConfig: DownloadConfig }

type MessageType =
    | Exit
    | Login of HttpClient option
    | PrepareDownloadView
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
      Client = None
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
    | Login newClient -> { model with Client = newClient }, Cmd.none
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
      //"OptionsViewCommand" |> Binding.cmd OptionsView ]
      ]

/// This is the application's entry point. It hands things off to Elmish.WPF
let main window =
    let program =
        Program.mkProgramWpf init update bindings
    //|> Program.withLogger (new SerilogLoggerFactory(logger))
    Program.startElmishLoop ElmConfig.Default window program
