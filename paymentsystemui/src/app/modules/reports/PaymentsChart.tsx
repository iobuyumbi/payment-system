import React, { useEffect, useRef, useState } from 'react'
import ApexCharts, { ApexOptions } from 'apexcharts'
import { useThemeMode } from '../../../_metronic/partials'
import { getCSSVariableValue } from '../../../_metronic/assets/ts/_utils'
import ReportService from '../../../services/ReportService'
import DeductiblePaymentService from '../../../services/DeductibleService';

type Props = {
  className: string
}
const deductibleService = new DeductiblePaymentService();
const reportService = new ReportService()

const PaymentsChart: React.FC<Props> = ({ className }) => {
  const chartRef = useRef<HTMLDivElement | null>(null)
  const { mode } = useThemeMode()
  const [chartData, setChartData] = useState<{ total: number; month: string }[]>([])

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await deductibleService.getChartData()
        if(response)
        setChartData(response) // Store data in state
      } catch (error) {
        console.error('Error fetching payment data:', error)
      }
    }

    fetchData()
  }, [])

  useEffect(() => {
    if (!chartRef.current || chartData.length === 0) return

    // Prepare chart options dynamically
    const chartOptions: ApexOptions = getChartOptions(chartData)

    // Destroy previous chart if exists
    if (chartRef.current.children.length > 0) {
      ApexCharts.exec('payments-chart', 'destroy')
    }

    // Initialize and render the new chart
    const chart = new ApexCharts(chartRef.current, chartOptions)
    chart.render()

    return () => {
      chart.destroy()
    }
  }, [chartData, mode])

  return (
    <div className={`${className}`}>
     
        <div ref={chartRef} id='kt_charts_widget_3_chart' style={{ height: '350px' }}></div>
      </div>
    
  )
}

export { PaymentsChart }

// Function to generate chart options dynamically
function getChartOptions(data: { total: number; month: string }[]): ApexOptions {
  const labelColor = getCSSVariableValue('--bs-gray-500')
  const borderColor = getCSSVariableValue('--bs-gray-200')
  const baseColor = getCSSVariableValue('--bs-info')
  const lightColor = getCSSVariableValue('--bs-info-light')

  return {
    series: [
      {
        name: 'Payments',
        data: data.map(payment => payment.total),
      },
    ],
    chart: {
      id: 'payments-chart',
      fontFamily: 'inherit',
      type: 'area',
      height: 350,
      toolbar: { show: false },
    },
    stroke: { curve: 'smooth', width: 3, colors: [baseColor] },
    xaxis: {
      categories: data.map(payment => payment.month),
      labels: { style: { colors: labelColor, fontSize: '12px' } },
    },
    yaxis: {
      labels: { style: { colors: labelColor, fontSize: '12px' } },
    },
    tooltip: {
      y: {
        formatter: val => `${val}`, // Format values dynamically
      },
    },
    grid: {
      borderColor: borderColor,
      strokeDashArray: 4,
    },
    colors: [lightColor],
  }
}
