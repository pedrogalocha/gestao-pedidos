import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BuscaFormComponent } from '../busca-form/busca-form.component';
import { ApiService, Pedido, ValorTotalResponse, Cliente } from '../../services/api.service';
import { finalize } from 'rxjs/operators';
import { MatCardModule } from '@angular/material/card';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-consultas',
  standalone: true,
  imports: [
    CommonModule,
    BuscaFormComponent,
    MatCardModule,
    MatExpansionModule,
    MatTableModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './consultas.component.html',
  styleUrls: ['./consultas.component.css']
})
export class ConsultasComponent implements OnInit {
  valorTotal: ValorTotalResponse | null = null;
  pedidos: Pedido[] = [];
  clientes: Cliente[] = [];
  isLoading = false;
  errorMessage: string | null = null;
  displayedColumns: string[] = ['produto', 'quantidade', 'precoUnitario', 'valorTotal'];

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.apiService.getAllClientes().subscribe(clientes => {
      this.clientes = clientes;
    });
  }

  handleBuscarPorCliente(clienteId: number): void {
    this.resetState();
    this.isLoading = true;

    this.apiService.getPedidosPorCliente(clienteId)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: data => this.pedidos = data,
        error: err => this.handleError(err)
      });
  }

  handleBuscarPorPedido(pedidoId: number): void {
    this.resetState();
    this.isLoading = true;

    this.apiService.getValorTotalPedido(pedidoId)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: data => this.valorTotal = data,
        error: err => this.handleError(err)
      });
  }

  private resetState(): void {
    this.valorTotal = null;
    this.pedidos = [];
    this.errorMessage = null;
  }

  private handleError(error: any): void {
    this.errorMessage = 'Erro ao buscar dados. Verifique o ID e tente novamente.';
    console.error(error);
  }
}
