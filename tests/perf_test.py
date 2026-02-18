"""
Performance test for BulkCreate + BulkUpdate mutations.
Simulates semi-realistic batch operations and measures response times.
"""
import json
import time
import urllib.request
import sys

API_URL = "http://localhost:64707/graphql"


def graphql(query):
    data = json.dumps({"query": query}).encode("utf-8")
    req = urllib.request.Request(API_URL, data=data, headers={"Content-Type": "application/json"})
    start = time.perf_counter()
    with urllib.request.urlopen(req, timeout=60) as resp:
        body = json.loads(resp.read())
    elapsed = time.perf_counter() - start
    return body, elapsed


def build_header_items(count):
    items = []
    for i in range(count):
        m = (i // 59) % 60
        s = i % 59
        item = (
            '{ logDateIn: "2026-02-18T10:%02d:%02dZ", '
            'logDateOut: "2026-02-18T10:%02d:%02dZ", '
            'logState: COMPLETED, logMethodUrl: "/api/perf/%d", '
            'microserviceName: "perf-svc", httpMethod: "POST" }'
        ) % (m, s, m, s + 1, i)
        items.append(item)
    return items


def run_test():
    batch_sizes = [10, 50, 100]

    print("=" * 70)
    print("  PERFORMANCE TEST - BulkCreate + BulkUpdate Mutations")
    print("=" * 70)

    for size in batch_sizes:
        print("")
        print("-" * 70)
        print("  BATCH SIZE: %d items" % size)
        print("-" * 70)

        # ===== 1) BulkCreate LogServicesHeader =====
        header_items = build_header_items(size)
        q = "mutation { bulkCreateLogServicesHeader(input: { items: [%s] }) { success totalRequested totalInserted totalFailed errorMessage insertedItems { logId } } }" % ", ".join(header_items)
        resp, t = graphql(q)
        if "errors" in resp:
            print("  [Header] BulkCreate  : GRAPHQL ERROR - %s" % resp["errors"][0]["message"])
            continue
        r = resp["data"]["bulkCreateLogServicesHeader"]
        if not r["success"]:
            print("  [Header] BulkCreate  : FAILED - %s" % r.get("errorMessage", "unknown"))
            continue
        print("  [Header] BulkCreate  : %8.1f ms | %d/%d inserted | %.1f ms/item" % (t * 1000, r["totalInserted"], size, t / size * 1000))

        # Get LogIds from the BulkCreate result
        log_ids = [item["logId"] for item in r.get("insertedItems", [])]

        # ===== 2) BulkUpdate LogServicesHeader =====
        update_items = []
        for i, lid in enumerate(log_ids):
            update_items.append('{ logId: %d, logState: FAILED, errorCode: "PERF-%04d", errorDescription: "Perf error %d", requestDuration: %d }' % (lid, i, i, 100 + i * 10))
        q = "mutation { bulkUpdateLogServicesHeader(input: { items: [%s] }) { success totalRequested totalUpdated totalFailed } }" % ", ".join(update_items)
        resp, t = graphql(q)
        r = resp["data"]["bulkUpdateLogServicesHeader"]
        print("  [Header] BulkUpdate  : %8.1f ms | %d/%d updated  | %.1f ms/item" % (t * 1000, r["totalUpdated"], size, t / size * 1000))

        # ===== 3) BulkCreate LogMicroservice =====
        ms_items = []
        for i, lid in enumerate(log_ids):
            m = (i // 59) % 60
            s = i % 59
            ms_items.append('{ logId: %d, requestId: %d, eventName: "PerfEvent%d", logDate: "2026-02-18T12:%02d:%02dZ", logLevel: "INFO", logMicroserviceText: "Perf log %d" }' % (lid, 1000 + i, i, m, s, i))
        q = "mutation { bulkCreateLogMicroservice(input: { items: [%s] }) { success totalRequested totalInserted totalFailed insertedItems { logMicroserviceId } } }" % ", ".join(ms_items)
        resp, t = graphql(q)
        r = resp["data"]["bulkCreateLogMicroservice"]
        print("  [Micro]  BulkCreate  : %8.1f ms | %d/%d inserted | %.1f ms/item" % (t * 1000, r["totalInserted"], size, t / size * 1000))

        # Get microservice IDs
        ms_ids = [item["logMicroserviceId"] for item in r.get("insertedItems", [])]

        # ===== 4) BulkUpdate LogMicroservice =====
        if ms_ids:
            ms_update_items = []
            for i, mid in enumerate(ms_ids):
                ms_update_items.append('{ logMicroserviceId: "%s", logLevel: "ERROR", logMicroserviceText: "Updated %d" }' % (mid, i))
            q = "mutation { bulkUpdateLogMicroservice(input: { items: [%s] }) { success totalRequested totalUpdated totalFailed } }" % ", ".join(ms_update_items)
            resp, t = graphql(q)
            r = resp["data"]["bulkUpdateLogMicroservice"]
            print("  [Micro]  BulkUpdate  : %8.1f ms | %d/%d updated  | %.1f ms/item" % (t * 1000, r["totalUpdated"], len(ms_ids), t / len(ms_ids) * 1000))

        # ===== 5) BulkCreate LogServicesContent =====
        content_items = []
        for i, lid in enumerate(log_ids):
            content_items.append('{ logId: %d, logServicesDate: "2026-02-18", logServicesLogLevel: "INFO", logServicesState: "OK", logServicesContentText: "Perf content %d" }' % (lid, i))
        q = "mutation { bulkCreateLogServicesContent(input: { items: [%s] }) { success totalRequested totalInserted totalFailed } }" % ", ".join(content_items)
        resp, t = graphql(q)
        r = resp["data"]["bulkCreateLogServicesContent"]
        print("  [Content] BulkCreate : %8.1f ms | %d/%d inserted | %.1f ms/item" % (t * 1000, r["totalInserted"], size, t / size * 1000))

        # ===== 6) BulkUpdate LogServicesContent =====
        content_update = []
        for i, lid in enumerate(log_ids):
            content_update.append('{ logId: %d, logServicesLogLevel: "WARN", logServicesState: "UPDATED", logServicesContentText: "Updated content %d" }' % (lid, i))
        q = "mutation { bulkUpdateLogServicesContent(input: { items: [%s] }) { success totalRequested totalUpdated totalFailed } }" % ", ".join(content_update)
        resp, t = graphql(q)
        r = resp["data"]["bulkUpdateLogServicesContent"]
        print("  [Content] BulkUpdate : %8.1f ms | %d/%d updated  | %.1f ms/item" % (t * 1000, r["totalUpdated"], size, t / size * 1000))

    print("")
    print("=" * 70)
    print("  TEST COMPLETE")
    print("=" * 70)


if __name__ == "__main__":
    run_test()
