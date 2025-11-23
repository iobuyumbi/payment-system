
export default function InfoPanel(props: any) {
    const { title, ContentComponent } = props;
    return (
        <div className="info-panel shadow">
            <div className="info-header">
                <h2 className="fs-3 plus-jakarta-sans-400">{title} </h2>
            </div>
            <ContentComponent />
        </div>
    )
}
