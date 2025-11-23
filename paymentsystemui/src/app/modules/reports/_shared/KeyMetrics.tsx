
const KeyMetrics = ({ keyMetrics, className }: any) => (
    <div className="row mb-4">
        {keyMetrics && keyMetrics.map((metric: any, index: any) => (
            <div className="col" key={index}>
                <div className={`card text-center ${className}`}>
                    <div className="card-body">
                        <h5 className="card-title text-gray-600">{metric.title}</h5>
                        <p className="card-text fs-2x">{metric.value}{metric.title === "Conversion Rate" && "%"} </p>
                    </div>
                </div>
            </div>
        ))}
    </div>
);

export default KeyMetrics;
