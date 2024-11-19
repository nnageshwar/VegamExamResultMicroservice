$(document).ready(function () {
    //The Microservice AP end point
    const apiBaseUrl = "https://examdashboardapi.azurewebsites.net";
    let gridTable;

    $("#showMarksDataBtn").click(function () {
        updateGridHeader("Marks Data");
        fetchAndRender(`${apiBaseUrl}/api/exam-results/total`, false, false);
    });

    $("#showResultsAllBtn").click(function () {
        updateGridHeader("Results of All");
        fetchAndRender(`${apiBaseUrl}/api/exam-results/total`, true, false);
    });

    $("#showResultsWithGraceBtn").click(function () {
        updateGridHeader("Results with Grace Marks");
        fetchAndRender(`${apiBaseUrl}/api/exam-results/with-grace-marks`, true, true);
    });

    //By default, let us show the total marks with results
    fetchAndRender(`${apiBaseUrl}/api/exam-results/total`, false, false);

    function updateGridHeader(headerText) {
        $("#gridHeader").text(headerText);
    }

    function fetchAndRender(apiUrl, showResult = false, showGrace = false) {
        $.get(apiUrl, function (data) {
            if (data.length === 0) {
                $("#alert-container").text("No data available.").show();
                return;
            }
            $("#alert-container").hide();
            populateTable(data, showResult, showGrace);
        }).fail(function () {
            $("#alert-container").text("Failed to fetch data from the API.").show();
        });
    }

    function populateTable(data, showResult = false, showGrace = false) {
        const tableHeader = $("#tableHeader");
        const tbody = $("#studentTable tbody");

        tableHeader.empty();
        tbody.empty();

        tableHeader.append("<th>Student Name</th>");
        const subjects = data[0] && data[0].marks ? Object.keys(data[0].marks) : [];
        subjects.forEach((subject) => {
            tableHeader.append(`<th>${subject}</th>`);
        });
        tableHeader.append("<th>Total</th>");

        if (showResult || showGrace)
            tableHeader.append("<th>Result</th>");
        if (showGrace)
            tableHeader.append("<th>Remarks</th>");

        data.forEach((student) => {
            let row = `<tr><td>${student.studentName}</td>`;
            subjects.forEach((subject) => {
                const marks = student.marks[subject] || 0;
                row += `<td>${marks}</td>`;
            });
            row += `<td>${student.total}</td>`;
            if (showResult || showGrace) {
                const resultClass = student.result === "Pass" ? "pass" : "fail";
                row += `<td class="${resultClass}">${student.result}</td>`;
            }
            if (showGrace) {
                row += `<td>${student.remarks || ""}</td>`;
            }
            row += `</tr>`;
            tbody.append(row);
        });

        if (!gridTable) {
            gridTable = $('#studentTable').DataTable({
                "paging": true,
                "pageLength": 10,
                "searching": true,
                "ordering": false,
                "order": [] //Let's not order by anything. Let's display the data provided by the API
            });
        } else {
            gridTable.clear();
            gridTable.rows.add(tbody.find('tr'));
            gridTable.draw();
        }
    }
});
