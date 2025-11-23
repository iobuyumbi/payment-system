import React, { useEffect, useState } from "react";
import ReactApexChart from "react-apexcharts";
import PaymentBatchService from "../../../services/PaymentBatchService";
import { ApexOptions } from "apexcharts";

const paymentBatchService = new PaymentBatchService();

const PaymentBatchPieChart = () => {
  const [chartData, setChartData] = useState<number[]>([]); // Ensuring correct type
  const [chartLabels, setChartLabels] = useState<string[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await paymentBatchService.getPaymentBatchStats();
        
        if (response && response.stageCounts) {
          const stageCount = response.stageCounts;

          const labels = Object.keys(stageCount);
          const values = Object.values(stageCount).map(Number); // Ensuring numbers

          setChartLabels(labels);
          setChartData(values);
        }
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, []);

  const chartOptions: ApexOptions = {
    chart: {
      type: "pie",
      height: 350,
    },
    labels: chartLabels,
    colors: ["#0088FE", "#00C49F", "#FFBB28", "#FF8042", "#A28BFF"],
    legend: {
      position: "bottom",
    },
    tooltip: {
      y: {
        formatter: (val: number) => `${val} Batches`,
      },
    },
  };

  return (
    <div className="card p-4">
      <h3 className="text-center">Payment Batches</h3>
      {chartData.length > 0 ? (
        <ReactApexChart options={chartOptions} series={chartData} type="pie" height={350} />
      ) : (
        <p className="text-center">Loading...</p>
      )}
    </div>
  );
};

export default PaymentBatchPieChart;
