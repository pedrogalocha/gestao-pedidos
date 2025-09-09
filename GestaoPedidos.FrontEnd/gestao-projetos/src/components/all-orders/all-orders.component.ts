import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService, PedidoDetalhado } from '../../services/api.service';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-all-orders',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './all-orders.component.html',
  styleUrls: ['./all-orders.component.css']
})
export class AllOrdersComponent implements OnInit {
  displayedColumns: string[] = ['codigoPedido', 'nomeCliente', 'quantidadeItens', 'valorTotal'];
  dataSource = new MatTableDataSource<PedidoDetalhado>();
  isLoading = false;
  errorMessage: string | null = null;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.loadAllPedidos();
  }

  loadAllPedidos() {
    this.isLoading = true;
    this.errorMessage = null;
    this.apiService.getAllPedidos().subscribe({
      next: (data: PedidoDetalhado[]) => {
        this.dataSource.data = data;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.isLoading = false;
      },
      error: (err: any) => {
        this.errorMessage = 'Erro ao carregar pedidos.';
        this.isLoading = false;
        console.error(err);
      }
    });
  }
}
