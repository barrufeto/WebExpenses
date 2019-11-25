﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExpensesEF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Gasto> Gasto { get; set; }
        public virtual DbSet<GastoRecurrente> GastoRecurrente { get; set; }
        public virtual DbSet<GrupoGasto> GrupoGasto { get; set; }
        public virtual DbSet<Idiomas> Idiomas { get; set; }
        public virtual DbSet<Periocidad> Periocidad { get; set; }
        public virtual DbSet<PeriocidadTextosTraduccion> PeriocidadTextosTraduccion { get; set; }
        public virtual DbSet<SubTipoGasto> SubTipoGasto { get; set; }
        public virtual DbSet<SubTipoGastoTextosTraduccion> SubTipoGastoTextosTraduccion { get; set; }
        public virtual DbSet<Sueldo> Sueldo { get; set; }
        public virtual DbSet<SummaryPorGrupoGasto> SummaryPorGrupoGasto { get; set; }
        public virtual DbSet<SummaryPorGrupoGasto_Mensual> SummaryPorGrupoGasto_Mensual { get; set; }
        public virtual DbSet<SummaryPorTipoGasto> SummaryPorTipoGasto { get; set; }
        public virtual DbSet<SummaryPorTipoGasto_Mensual> SummaryPorTipoGasto_Mensual { get; set; }
        public virtual DbSet<SummaryPorTipoPago> SummaryPorTipoPago { get; set; }
        public virtual DbSet<SummaryPorTipoPago_Mensual> SummaryPorTipoPago_Mensual { get; set; }
        public virtual DbSet<SummaryRegalo> SummaryRegalo { get; set; }
        public virtual DbSet<SummaryRegalo_Mensual> SummaryRegalo_Mensual { get; set; }
        public virtual DbSet<SummaryTotal_Mensual> SummaryTotal_Mensual { get; set; }
        public virtual DbSet<TextosVarios> TextosVarios { get; set; }
        public virtual DbSet<TipoGasto> TipoGasto { get; set; }
        public virtual DbSet<TipoGastoTextosTraduccion> TipoGastoTextosTraduccion { get; set; }
        public virtual DbSet<TipoPago> TipoPago { get; set; }
        public virtual DbSet<AggregatedCounter> AggregatedCounter { get; set; }
        public virtual DbSet<Hash> Hash { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<JobParameter> JobParameter { get; set; }
        public virtual DbSet<JobQueue> JobQueue { get; set; }
        public virtual DbSet<List> List { get; set; }
        public virtual DbSet<Schema> Schema { get; set; }
        public virtual DbSet<Server> Server { get; set; }
        public virtual DbSet<Set> Set { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<GrupoGastoTextosTraduccion> GrupoGastoTextosTraduccion { get; set; }
        public virtual DbSet<TipoPagoTextosTraduccion> TipoPagoTextosTraduccion { get; set; }
        public virtual DbSet<Counter> Counter { get; set; }
        public virtual DbSet<vst_GastosRecurrentes> vst_GastosRecurrentes { get; set; }
        public virtual DbSet<vst_InOut> vst_InOut { get; set; }
        public virtual DbSet<SummaryPorSubTipoGasto> SummaryPorSubTipoGasto { get; set; }
        public virtual DbSet<SummaryPorSubTipoGasto_Mensual> SummaryPorSubTipoGasto_Mensual { get; set; }
        public virtual DbSet<GastoCompartido> GastoCompartido { get; set; }
        public virtual DbSet<ValuesDefaultConcepto> ValuesDefaultConcepto { get; set; }
        public virtual DbSet<ValuesEnvioEmail> ValuesEnvioEmail { get; set; }
        public virtual DbSet<vst_Export_Gastos> vst_Export_Gastos { get; set; }
    
        public virtual int spGetMonthlyExpenses(string userGasto, Nullable<int> month, Nullable<int> year, ObjectParameter retMont, ObjectParameter retMontPrevious, ObjectParameter retCurrentCard, ObjectParameter retMontIncludingComputables)
        {
            var userGastoParameter = userGasto != null ?
                new ObjectParameter("UserGasto", userGasto) :
                new ObjectParameter("UserGasto", typeof(string));
    
            var monthParameter = month.HasValue ?
                new ObjectParameter("month", month) :
                new ObjectParameter("month", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spGetMonthlyExpenses", userGastoParameter, monthParameter, yearParameter, retMont, retMontPrevious, retCurrentCard, retMontIncludingComputables);
        }
    
        public virtual int spChartGetExpenses(string userGasto, Nullable<int> month, Nullable<int> year, Nullable<int> idioma, Nullable<int> tipoDatos, ObjectParameter resGastos, ObjectParameter resIn)
        {
            var userGastoParameter = userGasto != null ?
                new ObjectParameter("UserGasto", userGasto) :
                new ObjectParameter("UserGasto", typeof(string));
    
            var monthParameter = month.HasValue ?
                new ObjectParameter("month", month) :
                new ObjectParameter("month", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            var idiomaParameter = idioma.HasValue ?
                new ObjectParameter("Idioma", idioma) :
                new ObjectParameter("Idioma", typeof(int));
    
            var tipoDatosParameter = tipoDatos.HasValue ?
                new ObjectParameter("TipoDatos", tipoDatos) :
                new ObjectParameter("TipoDatos", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spChartGetExpenses", userGastoParameter, monthParameter, yearParameter, idiomaParameter, tipoDatosParameter, resGastos, resIn);
        }
    
        public virtual int spSummarizeExpenses(Nullable<int> month, Nullable<int> year)
        {
            var monthParameter = month.HasValue ?
                new ObjectParameter("month", month) :
                new ObjectParameter("month", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spSummarizeExpenses", monthParameter, yearParameter);
        }
    
        public virtual int spSummarizeExpenses_GrupoGastos(Nullable<int> year)
        {
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spSummarizeExpenses_GrupoGastos", yearParameter);
        }
    
        public virtual int spBloqueaEdicionGastos(Nullable<int> month, Nullable<int> year)
        {
            var monthParameter = month.HasValue ?
                new ObjectParameter("month", month) :
                new ObjectParameter("month", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spBloqueaEdicionGastos", monthParameter, yearParameter);
        }
    
        public virtual int spGetSueldo(string userGasto, Nullable<int> month, ObjectParameter retSueldo)
        {
            var userGastoParameter = userGasto != null ?
                new ObjectParameter("UserGasto", userGasto) :
                new ObjectParameter("UserGasto", typeof(string));
    
            var monthParameter = month.HasValue ?
                new ObjectParameter("month", month) :
                new ObjectParameter("month", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spGetSueldo", userGastoParameter, monthParameter, retSueldo);
        }
    
        public virtual int spSummarizeExpenses_GrupoGastosMensual(Nullable<int> month, Nullable<int> year)
        {
            var monthParameter = month.HasValue ?
                new ObjectParameter("month", month) :
                new ObjectParameter("month", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spSummarizeExpenses_GrupoGastosMensual", monthParameter, yearParameter);
        }
    
        public virtual int spSummarizeExpenses_Mensual(Nullable<int> month, Nullable<int> year)
        {
            var monthParameter = month.HasValue ?
                new ObjectParameter("month", month) :
                new ObjectParameter("month", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spSummarizeExpenses_Mensual", monthParameter, yearParameter);
        }
    
        public virtual int spSummarizeExpenses_TipoGastos(Nullable<int> year)
        {
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spSummarizeExpenses_TipoGastos", yearParameter);
        }
    
        public virtual int spSummarizeExpenses_TipoGastosMensual(Nullable<int> month, Nullable<int> year)
        {
            var monthParameter = month.HasValue ?
                new ObjectParameter("month", month) :
                new ObjectParameter("month", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spSummarizeExpenses_TipoGastosMensual", monthParameter, yearParameter);
        }
    
        public virtual int spSummarizeExpenses_TipoPago(Nullable<int> year)
        {
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spSummarizeExpenses_TipoPago", yearParameter);
        }
    
        public virtual int spSummarizeExpenses_TipoPagoMensual(Nullable<int> month, Nullable<int> year)
        {
            var monthParameter = month.HasValue ?
                new ObjectParameter("month", month) :
                new ObjectParameter("month", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spSummarizeExpenses_TipoPagoMensual", monthParameter, yearParameter);
        }
    
        public virtual int spSummarizeExpenses_Regalo(Nullable<int> year)
        {
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spSummarizeExpenses_Regalo", yearParameter);
        }
    
        public virtual int spSummarizeExpenses_RegaloMensual(Nullable<int> month, Nullable<int> year)
        {
            var monthParameter = month.HasValue ?
                new ObjectParameter("month", month) :
                new ObjectParameter("month", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spSummarizeExpenses_RegaloMensual", monthParameter, yearParameter);
        }
    
        public virtual int spSummarizeExpenses_SubTipoGastos(Nullable<int> year)
        {
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spSummarizeExpenses_SubTipoGastos", yearParameter);
        }
    
        public virtual int spSummarizeExpenses_SubTipoGastosMensual(Nullable<int> month, Nullable<int> year)
        {
            var monthParameter = month.HasValue ?
                new ObjectParameter("month", month) :
                new ObjectParameter("month", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spSummarizeExpenses_SubTipoGastosMensual", monthParameter, yearParameter);
        }
    }
}
