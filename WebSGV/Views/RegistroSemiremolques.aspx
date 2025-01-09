<%@ Page Title="Registro de Semiremolque" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistroSemiremolques.aspx.cs" Inherits="WebSGV.Views.RegistroSemiremolques" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center vh-100">
        <div class="card registro-semi-card w-50 shadow-lg">
            <div class="card-header text-center registro-semi-header">
                <h2 class="header mb-0 text-white">Registro de Semiremolque</h2>
            </div>
            <div class="card-body p-4">
                <form>
                    <!-- Campo N° Placa -->
                    <div class="row mb-3">
                        <div class="col-md-12 form-group">
                            <label for="txtPlaca" class="form-label">
                                <i class="fas fa-car"></i> N° Placa:
                            </label>
                            <input type="text" id="txtPlaca" class="form-control" placeholder="Ingrese N° de placa">
                        </div>
                    </div>
                    <!-- Campo Modelo -->
                    <div class="row mb-3">
                        <div class="col-md-12 form-group">
                            <label for="txtModelo" class="form-label">
                                <i class="fas fa-cogs"></i> Modelo:
                            </label>
                            <input type="text" id="txtModelo" class="form-control" placeholder="Ingrese modelo">
                        </div>
                    </div>
                    <!-- Campo Marca -->
                    <div class="row mb-3">
                        <div class="col-md-12 form-group">
                            <label for="txtMarca" class="form-label">
                                <i class="fas fa-industry"></i> Marca:
                            </label>
                            <input type="text" id="txtMarca" class="form-control" placeholder="Ingrese marca">
                        </div>
                    </div>
                    <!-- Botón -->
                    <div class="text-center">
                        <button type="submit" class="btn btn-primary btn-lg btn-block registro-semi-btn">
                            <i class="fas fa-save"></i> Registrar
                        </button>
                    </div>
                </form>
            </div>
        </div>
    </div>
</asp:Content>
